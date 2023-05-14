using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClipboardObserver.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public sealed class AwsCredentialsFile
    {
        public IServiceProvider Services { get; }

        public AwsCredentialsConfigOptions Options { get; }

        private FileInfo CredentialsFile { get; }

        public string FullName => CredentialsFile?.FullName ?? "[unspecified]";

        private List<AwsCredentials> Profiles { get; set; }

        public AwsCredentials GetDefaultProfile() => Profiles?.FirstOrDefault(p=>p.IsDefault());

        public AwsCredentials GetProfileByName(string userName) => Profiles.FirstOrDefault(p => p.UserName.ToLowerInvariant() == userName.ToLowerInvariant());

        public void AddOrUpdateProfile(AwsCredentials credentials)
        {
            var toUpdate = GetProfileByName(credentials.UserName);
            if (toUpdate == null)
            {
                toUpdate = Services.GetRequiredService<AwsCredentials>();
                Profiles.Add(toUpdate);
            }
            toUpdate.UpdateFromProfile(credentials);
        }
        
        public AwsCredentialsFile(
            IServiceProvider services,
            IOptionsMonitor<AwsCredentialsConfigOptions> options)
        {
            Services = services;
            Options = options.CurrentValue;
            var awsConfigFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".aws");
            var awsCredentialFileName = Path.Combine(awsConfigFolderPath, "credentials");
            CredentialsFile = new FileInfo(awsCredentialFileName);
            if (!Directory.Exists(awsConfigFolderPath)) 
            {
                Directory.CreateDirectory(awsConfigFolderPath);
            }
        }

        public async Task LoadFile()
        {
            Profiles = await ParseCredentialFile(CredentialsFile);
        }

        public async Task SaveFile()
        {
            await using var credentialsWriter = CredentialsFile.CreateText();
            foreach(var p in Profiles)
            {
                await credentialsWriter.WriteLineAsync(p.ToString());
            }
            await credentialsWriter.FlushAsync();
        }

        private async Task<List<AwsCredentials>> ParseCredentialFile(FileInfo credFile)
        {
            var input = await File.ReadAllLinesAsync(credFile.FullName);

            List<string> profileSections = new();
            StringBuilder profileSection = new();
            bool hasStartedReadingProfiles = false;

            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line.Trim()))
                {
                    continue;
                }

                if (AwsCredentials.RegexPatterns.UserNameMatcher.IsMatch(line))
                {
                    if (hasStartedReadingProfiles)
                    {
                        profileSections.Add(profileSection.ToString());
                        profileSection = new StringBuilder();
                    }
                    else
                    {
                        hasStartedReadingProfiles = true;
                    }
                }

                if (hasStartedReadingProfiles)
                {
                    profileSection.AppendLine(line);
                }
            }

            if (hasStartedReadingProfiles)
            {
                profileSections.Add(profileSection.ToString());
            }

            var profiles = profileSections
                .Select(ps => 
                    Services
                        .GetRequiredService<AwsCredentials>()
                        .FromProfileSection(ps)
                    )
                .ToList();
            return profiles;
        }
    }
}
