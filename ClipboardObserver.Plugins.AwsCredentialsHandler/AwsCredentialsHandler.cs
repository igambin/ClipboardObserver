using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        public IServiceProvider Services { get; }
        private SharpClipboard Clipboard { get; }
        public AwsCredentialsFile AwsCredentialsFile { get; }

        private AwsCredentialsConfigOptions Options { get; }
        
        public List<SharpClipboard.ContentTypes> TriggeredBy { get; } = new()
        {
            SharpClipboard.ContentTypes.Text
        };

        public bool IsActive { get; set; }

        public AwsCredentialsHandler(
            IServiceProvider services,
            SharpClipboard clipboard, 
            AwsCredentialsFile awsCredentialsFile,
            IOptionsMonitor<AwsCredentialsConfigOptions> options)
        {
            Options = options.CurrentValue;
            Services = services;
            Clipboard = clipboard;
            AwsCredentialsFile = awsCredentialsFile;
        }

        public async Task ClipboardChanged()
        {
            if (!IsActive) return;

            var input = Clipboard.ClipboardText;

            AwsCredentials awsCredentials = Services.GetRequiredService<AwsCredentials>().FromProfileSection(input);

            if (awsCredentials.IsValid())
            {
                List<Task> tasks = new();

                if (Options.StoreCredentialsInFile)
                {
                    tasks.Add(StoreCredentialsInFile(awsCredentials));
                }

                if (Options.ExportCredentialsToEnv)
                {
                    tasks.Add(ExportCredentialsToEnv(awsCredentials));
                }

                await Task.WhenAll(tasks);
            }
        }

        private async Task ExportCredentialsToEnv(AwsCredentials credentials)
        {
            await Task.Delay(100);
        }

        private async Task StoreCredentialsInFile(AwsCredentials copiedCredentials)
        {
            try
            {
                await AwsCredentialsFile.LoadFile();

                AwsCredentialsFile.AddOrUpdateProfile(copiedCredentials);
                if (Options.CloneCredentialsToDefault)
                {
                    var defaultCredentials = Services.GetRequiredService<AwsCredentials>().UpdateFromProfile(copiedCredentials, "default");
                    AwsCredentialsFile.AddOrUpdateProfile(defaultCredentials);
                }

                await AwsCredentialsFile.SaveFile();
               
                OnClipboardProcessed($"File '{AwsCredentialsFile.FullName}' successfully written!");

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
                OnClipboardProcessed($"Writing file '{AwsCredentialsFile.FullName}' failed: " + ex.Message);
            }
        }

        public void OnClipboardProcessed(string message)
        {
            ClipboardEntryProcessed?.Invoke(this, new ClipboardEntryProcessedEventArgs {Handler = this, Message = message});
        }
    }
}
