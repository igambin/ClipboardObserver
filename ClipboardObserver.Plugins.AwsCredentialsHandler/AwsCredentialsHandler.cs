using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClipboardObserver.PluginManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WK.Libraries.SharpClipboardNS;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public class AwsCredentialsHandler : IClipboardChangedHandler
    {
        public event ClipboardEntryProcessedEventHandler ClipboardEntryProcessed;

        private IServiceProvider Services { get; }
        private SharpClipboard Clipboard { get; }
        private AwsCredentialsFile AwsCredentialsFile { get; }
        private AwsCredentialsConfigOptions Options { get; }
        private string _lastClipboardContent = string.Empty;

        public AwsCredentialsHandler(
            IServiceProvider services,
            SharpClipboard clipboard,
            AwsCredentialsFile awsCredentialsFile,
            IOptionsMonitor<AwsCredentialsConfigOptions> options
            ) {
            Clipboard = clipboard;
            Services = services;
            AwsCredentialsFile = awsCredentialsFile;
            Options = options.CurrentValue;
            clipboard.ClipboardChanged += async (o, args) => await ClipboardChanged();
        }
        
        private string Username { get; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

        public List<SharpClipboard.ContentTypes> TriggeredBy { get; } = [SharpClipboard.ContentTypes.Text];

        public bool IsActive { get; set; }

        public async Task ClipboardChanged()
        {
            if (!IsActive) return;

            var input = Clipboard.ClipboardText;

            var awsCredentials = Services.GetRequiredService<AwsCredentials>().FromProfileSection(input);

            if (awsCredentials.IsValid())
            {
                if (Options.StoreCredentialsInFile)
                {
                    await StoreCredentialsInFile(awsCredentials);
                }

                if (Options.ExportCredentialsToEnv)
                {
                    ExportCredentialsToEnv(awsCredentials);
                }
            }
        }

        private void ExportCredentialsToEnv(AwsCredentials credentials)
        {
            Task.Run(() =>
            {
                Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", credentials.AwsAccessKeyId, EnvironmentVariableTarget.User);
                Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", credentials.AwsSecretAccessKey, EnvironmentVariableTarget.User);
                Environment.SetEnvironmentVariable("AWS_SESSION_TOKEN", credentials.AwsSessionToken, EnvironmentVariableTarget.User);
            });
            OnClipboardProcessed(
                $"Credentials of [{credentials.UserName}] set to environment variables for user '{Username}'");
        }

        private async Task StoreCredentialsInFile(AwsCredentials copiedCredentials)
        {
            try
            {
                await AwsCredentialsFile.LoadFile();
                
                AwsCredentialsFile.AddOrUpdateProfile(copiedCredentials);

                if (Options.CloneCredentialsToDefault || Options.WriteDefaultProfileOnly)
                {
                    var defaultCredentials = Services.GetRequiredService<AwsCredentials>().UpdateFromProfile(copiedCredentials, "default");
                    AwsCredentialsFile.AddOrUpdateProfile(defaultCredentials);
                }

                await AwsCredentialsFile.SaveFile();

                OnClipboardProcessed($"Credentials of [{copiedCredentials.UserName}] successfully written to file '{AwsCredentialsFile.CredentialsFileName}'!");
                
                if (Options.WriteRegionToConfigFile)
                {
                    var configFile = Path.Combine(Options.AwsCredentialsFullPath, "config");
                    if (!File.Exists(configFile))
                    {
                        await using var configWriter = File.CreateText(configFile);
                        await configWriter.WriteLineAsync("[default]");
                        await configWriter.WriteLineAsync(
                            $"region = {copiedCredentials.Region ?? Options.DefaultRegion}");
                        await configWriter.FlushAsync();
                        OnClipboardProcessed($"File '{configFile}' successfully added!");
                    }
                }
            }
            catch (Exception ex)
            {
                OnClipboardProcessed($"Writing file '{AwsCredentialsFile.CredentialsFileName}' failed: " + ex.Message, ClipboardProcessingEventSeverity.Error);
            }
        }

        public void OnClipboardProcessed(string message, ClipboardProcessingEventSeverity severity = ClipboardProcessingEventSeverity.Info)
        {
            ClipboardEntryProcessed?.Invoke(this, new ClipboardEntryProcessedEventArgs {Handler = this, Message = message, Severity = severity});
        }
    }
}
