using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public string CredentialsFileName { get; init; } = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.UserProfile
            ),
            ".aws",
            "credentials"
        );

        private List<AwsCredentials> Profiles { get; set; }

        public AwsCredentials GetDefaultProfile()
            => Profiles?.FirstOrDefault(p => p.IsDefault());

        private AwsCredentials GetProfileByName(string userName)
            => Profiles.FirstOrDefault(
                p => string.Equals(p.UserName, userName, StringComparison.InvariantCultureIgnoreCase)
            );

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


        public async Task LoadFile()
        {
            Profiles = await ParseCredentialFile();
        }

        public async Task SaveFile()
        {
            FileInfo credentialsFile = new(CredentialsFileName);

            // Retry logic to handle IOException when file is in use
            const int maxRetries = 3;
            const int delayMs = 200;
            int retries = 0;

            while (true)
            {
                try
                {
                    await using var credentialsWriter = credentialsFile.CreateText();
                    foreach (
                        var p in
                        Profiles.Where(p => !Options.WriteDefaultProfileOnly || p.IsDefault()
                        )
                    )
                    {
                        await credentialsWriter.WriteLineAsync(p.ToString());
                    }
                    await credentialsWriter.FlushAsync();
                    break; // Success, exit loop
                }
                catch (IOException ex)
                {
                    retries++;
                    if (retries >= maxRetries)
                        throw new IOException($"Could not write to credentials file after {maxRetries} attempts: {ex.Message}", ex);

                    await Task.Delay(delayMs);
                }
            }
        }

        private async Task<List<AwsCredentials>> ParseCredentialFile()
        {
            FileInfo credentialsFile = new(CredentialsFileName);
            try
            {
                if (credentialsFile.Directory is { Exists: false })
                {
                    credentialsFile.Directory.Create();
                }

                if (!credentialsFile.Exists)
                {
                    await using var _ = File.Create(credentialsFile.FullName);
                    return [];
                }

            }
            catch (Exception e)
            {
                throw new Exception($"Error creating credentials file: {e.Message}");
            }

            var lines = await ReadFile();
            var profileSections = ParseProfileSections(lines);
            var profiles = ConvertProfilesToAwsCredentials(profileSections);
            return profiles;
        }

        private List<AwsCredentials> ConvertProfilesToAwsCredentials(List<string> profileSections)
            => profileSections
                .Select(ps =>
                    Services
                        .GetRequiredService<AwsCredentials>()
                        .FromProfileSection(ps)
                )
                .ToList();


        private List<string> ParseProfileSections(List<string> lines)
        {
            List<string> profileSections = [];

            var profileSection = new StringBuilder();
            var hasStartedReadingProfiles = false;
            foreach (var line in lines.Where(line => !string.IsNullOrWhiteSpace(line.Trim())))
            {
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

            return profileSections;
        }

        private async Task<List<string>> ReadFile()
        {
            FileInfo credentialsFile = new(CredentialsFileName);
            var lines = new List<string>();
            using var credentialsReader = credentialsFile.OpenText();
            while (!credentialsReader.EndOfStream)
            {
                lines.Add(await credentialsReader.ReadLineAsync());
            }
            credentialsReader.Close();
            return lines;
        }
    }
}
