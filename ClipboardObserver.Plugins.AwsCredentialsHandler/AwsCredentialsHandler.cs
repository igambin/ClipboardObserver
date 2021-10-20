using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ClipboardObserver.PluginManagement;
using WK.Libraries.SharpClipboardNS;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public class AwsCredentialsHandler : IClipboardChangedHandler
    {
        public event ClipboardEntryProcessedEventHandler ClipboardEntryProcessed;

        private SharpClipboard Clipboard { get; }
        
        public string Name { get; } = nameof(AwsCredentialsHandler);

        public SharpClipboard.ContentTypes ContentType { get; } = SharpClipboard.ContentTypes.Text;

        public AwsCredentialsHandler(SharpClipboard clipboard)
        {
            Clipboard = clipboard;
            Clipboard.ClipboardChanged += ClipboardChanged;
        }

        static readonly Regex NamePattern = new Regex(@"\[[a-zA-Z0-9_-]+\]");

        public void ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            if (e.ContentType == ContentType)
            {
                try
                {
                    var input = Clipboard.ClipboardText;
                    var lines = input.Split(Environment.NewLine);

                    var userLine = lines.SingleOrDefault(l => NamePattern.IsMatch(l));
                    var keyLines = lines.Select(l => l.Split("=")).Where(l => l.Length == 2).ToList();
                    var hasAllKeys =    keyLines.Any(l => l[0].Trim() == "aws_access_key_id")
                                    &&  keyLines.Any(l => l[0].Trim() == "aws_secret_access_key")
                                    &&  keyLines.Any(l => l[0].Trim() == "aws_session_token");

                    if (userLine != null && keyLines.Count == 3 && hasAllKeys)
                    {
                        var regionLine = "region = eu-central-1";

                        var awsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                            ".aws");
                        Directory.CreateDirectory(awsPath);

                        var credentialFile = Path.Combine(awsPath, "credentials");
                        using (var streamWriter = File.CreateText(credentialFile))
                        {

                            streamWriter.WriteLine(userLine.Trim());
                            streamWriter.WriteLine(regionLine);
                            keyLines.Select(k => string.Join('=', k)).ToList().ForEach(streamWriter.WriteLine);
                            streamWriter.WriteLine("[default]");
                            streamWriter.WriteLine(regionLine);
                            keyLines.Select(k => string.Join('=', k)).ToList().ForEach(streamWriter.WriteLine);
                            streamWriter.Flush();
                            OnClipboardProcessed($"File '{credentialFile}' successfully written!");
                        }

                        var configFile = Path.Combine(awsPath, "config");
                        if (!File.Exists(configFile))
                        {
                            using (var streamWriter = File.CreateText(configFile))
                            {
                                streamWriter.WriteLine(userLine.Trim());
                                streamWriter.WriteLine(regionLine);
                                streamWriter.WriteLine("[default]");
                                streamWriter.WriteLine(regionLine);
                                streamWriter.Flush();
                                OnClipboardProcessed($"File '{configFile}' successfully added!");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnClipboardProcessed("Writing file '.aws\\credentials' failed: " + ex.Message);
                }
            }
        }

        public void OnClipboardProcessed(string message)
        {
            if (ClipboardEntryProcessed != null)
            {
                ClipboardEntryProcessed(this, new ClipboardEntryProcessedEventArgs {Handler = Name, Message = message});
            }
        }
    }
}
