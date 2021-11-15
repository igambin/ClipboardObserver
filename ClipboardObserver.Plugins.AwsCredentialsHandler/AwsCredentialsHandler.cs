using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClipboardObserver.PluginManagement;
using Microsoft.Extensions.Options;
using WK.Libraries.SharpClipboardNS;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public class AwsCredentialsHandler : IClipboardChangedHandler
    {
        public event ClipboardEntryProcessedEventHandler ClipboardEntryProcessed;

        private SharpClipboard Clipboard { get; }
        
        private AwsCredentialsConfigOptions Options { get; }
        
        public List<SharpClipboard.ContentTypes> TriggeredBy { get; } = new()
        {
            SharpClipboard.ContentTypes.Text
        };

        public bool IsActive { get; set; }

        public AwsCredentialsHandler(
            SharpClipboard clipboard, 
            IOptionsMonitor<AwsCredentialsConfigOptions> options)
        {
            Options = options.CurrentValue;
            Clipboard = clipboard;
        }

        static readonly Regex NamePattern = new(@"\[[a-zA-Z0-9_-]+\]");

        public async Task ClipboardChanged()
        {
            if (!IsActive) return;

            var input = Clipboard.ClipboardText;

            var lines = input.Split(Environment.NewLine);

            var userLine = lines.SingleOrDefault(l => NamePattern.IsMatch(l))?.Trim();

            var keyLines =
                lines
                    .Select(l => l.Split("="))
                    .Where(l => l.Length == 2)
                    .ToList();

            AwsCredentials awsCredentials = new(){
                User               = userLine,
                Region             = Options.DefaultRegion,
                AwsAccessKeyId     = keyLines.FirstOrDefault(l => l[0].Trim().ToLower() == "aws_access_key_id")?[1].Trim(),
                AwsSecretAccessKey = keyLines.FirstOrDefault(l => l[0].Trim().ToLower() == "aws_secret_access_key")?[1].Trim(),
                AwsSessionToken    = keyLines.FirstOrDefault(l => l[0].Trim().ToLower() == "aws_session_token")?[1].Trim()
            };

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

        private class AwsCredentials
        {
            public string User { get; set; }
            public string Region { get; set; }
            public string AwsAccessKeyId { get; set; }
            public string AwsSecretAccessKey { get; set; }
            public string AwsSessionToken { get; set; }

            public string ToString(bool includeRegion = false, bool defaultUser = false)
            {
                StringBuilder sb = new();
                sb.AppendLine(defaultUser ? "[default]" : User);
                sb.AppendLine($"aws_access_key_id = {AwsAccessKeyId}");
                sb.AppendLine($"aws_secret_access_key = {AwsSecretAccessKey}");
                sb.AppendLine($"aws_session_token = {AwsSessionToken}");
                if (includeRegion) sb.AppendLine($"region = {Region}");
                return sb.ToString();
            }
        }

        private async Task ExportCredentialsToEnv(AwsCredentials credentials)
        {
            await Task.Delay(100);
        }

        private async Task StoreCredentialsInFile(AwsCredentials credentials)
        {
            var credentialFile = Path.Combine(Options.AwsCredentialsFullPath, "credentials");

            try
            {

                if (credentials.User != null 
                    && credentials.AwsAccessKeyId != null
                    && credentials.AwsSecretAccessKey != null
                    && credentials.AwsSessionToken != null
                )
                {
                    Directory.CreateDirectory(Options.AwsCredentialsFullPath);
                    await using var credentialWriter = File.CreateText(credentialFile);
                    await credentialWriter.WriteLineAsync(credentials.ToString(Options.AddRegionToCredentialsFile));
                    if (Options.CloneCredentialsToDefault)
                    {
                        await credentialWriter.WriteLineAsync(credentials.ToString(Options.AddRegionToCredentialsFile, true));
                    }

                    await credentialWriter.FlushAsync();
                    OnClipboardProcessed($"File '{credentialFile}' successfully written!");

                    if (Options.WriteRegionToConfigFile)
                    {
                        var configFile = Path.Combine(Options.AwsCredentialsFullPath, "config");
                        if (!File.Exists(configFile))
                        {
                            await using var configWriter = File.CreateText(configFile);
                            await configWriter.WriteLineAsync("[default]");
                            await configWriter.WriteLineAsync(
                                $"region = {credentials.Region ?? Options.DefaultRegion}");
                            await configWriter.FlushAsync();
                            OnClipboardProcessed($"File '{configFile}' successfully added!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnClipboardProcessed($"Writing file '{credentialFile}' failed: " + ex.Message);
            }
        }

        public void OnClipboardProcessed(string message)
        {
            ClipboardEntryProcessed?.Invoke(this, new ClipboardEntryProcessedEventArgs {Handler = this, Message = message});
        }
    }
}
