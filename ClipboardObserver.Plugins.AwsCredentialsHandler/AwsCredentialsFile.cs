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
    public sealed class AwsCredentialsFile(
        IServiceProvider services,
        IOptionsMonitor<AwsCredentialsConfigOptions> options)
    {
        public IServiceProvider Services { get; } = services;

        public AwsCredentialsConfigOptions Options { get; } = options.CurrentValue;


        private static FileInfo File { get; } = new (Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".aws", "credentials"));

        private static DirectoryInfo Folder { get; } = File.Directory;

        public string FullPath { get; } = File?.FullName ?? "[unspecified]";

        private List<AwsCredentials> Profiles { get; set; }

        public AwsCredentials GetDefaultProfile() => Profiles?.FirstOrDefault(p=>p.IsDefault());

        public AwsCredentials GetProfileByName(string userName) => Profiles.FirstOrDefault(p => string.Equals(p.UserName, userName, StringComparison.InvariantCultureIgnoreCase));

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

        private static void EnsureCredentialsFileExists()
        {
            if (!Folder.Exists)
            {
                Folder.Create();
            }

            if (File.Exists) return;

            using (var fs = new FileStream(File.FullName, FileMode.Create, FileAccess.Write, FileShare.None));

        }

        public async Task LoadFile()
        {
            EnsureCredentialsFileExists();
            Profiles = await ParseCredentialFile(File);
        }

        public async Task SaveFile()
        {
            await using var credentialsWriter = File.CreateText();
            foreach(var p in Profiles)
            {
                await credentialsWriter.WriteLineAsync(p.ToString());
            }
            await credentialsWriter.FlushAsync();
        }

        private async Task<List<AwsCredentials>> ParseCredentialFile(FileInfo credFile)
        {
            var input = await System.IO.File.ReadAllLinesAsync(credFile.FullName);

            List<string> profileSections = [];
            var profileSection = new StringBuilder();
            var hasStartedReadingProfiles = false;

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
