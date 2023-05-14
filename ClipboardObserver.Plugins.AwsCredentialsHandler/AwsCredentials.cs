using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ClipboardObserver.Common;
using Microsoft.Extensions.Options;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public sealed class AwsCredentials
    {
        public AwsCredentialsConfigOptions Options { get; }
        private const string DefaultProfileName = "default";

        public sealed class RegexPatterns
        {
            private static readonly Regex UserNamePattern = new(@"\s*\[(?<profilename>[a-zA-Z0-9_-]+)\]\s*");
            private static readonly Regex RegionPattern = new(@"\s*region\s*=\s*(?<regionname>[\w-]+)\s*");
            private static readonly Regex AwsAccessKeyIdPattern = new(@"\s*aws_access_key_id\s*=\s*(?<accesskey>\w+)\s*");
            private static readonly Regex AwsSecretAccessKeyPattern = new(@"\s*aws_secret_access_key\s*=\s*(?<secretkey>[\w/+]+)\s*");
            private static readonly Regex AwsSessionTokenPattern = new(@"\s*aws_session_token\s*=\s*(?<sessiontoken>[\w/+]+)\s*");
            public static readonly RegExMatcher UserNameMatcher = new(UserNamePattern);
            public static readonly RegExPattern Name = new(UserNamePattern, (groups) => groups["profilename"].ToString());
            public static readonly RegExPattern Region = new(RegionPattern, (groups) => groups["regionname"].ToString());
            public static readonly RegExPattern AccessKeyId = new(AwsAccessKeyIdPattern, (groups) => groups["accesskey"].ToString());
            public static readonly RegExPattern SecretAccessKey = new(AwsSecretAccessKeyPattern, (groups) => groups["secretkey"].ToString());
            public static readonly RegExPattern SessionToken = new(AwsSessionTokenPattern, (groups) => groups["sessiontoken"].ToString());
        }

        public string UserName { get; set; }
        public string Region { get; set; }
        public string AwsAccessKeyId { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string AwsSessionToken { get; set; }

        public AwsCredentials(
            IOptionsMonitor<AwsCredentialsConfigOptions> options
            )
        {
            Options = options.CurrentValue;
        }

        public AwsCredentials UpdateFromProfile(AwsCredentials profile, string newUserName = null)
        {
            UserName = newUserName ?? profile.UserName;
            Region = profile.Region;
            if(string.IsNullOrWhiteSpace(Region)) Region = Options.DefaultRegion;
            AwsAccessKeyId = profile.AwsAccessKeyId;
            AwsSecretAccessKey = profile.AwsSecretAccessKey;
            AwsSessionToken = profile.AwsSessionToken;
            return this;
        }

        public AwsCredentials FromProfileSection(string input) 
        {
            UserName = RegexPatterns.Name.Transform(input);
            Region = RegexPatterns.Region.Transform(input);
            if (string.IsNullOrWhiteSpace(Region)) Region = Options.DefaultRegion;
            AwsAccessKeyId = RegexPatterns.AccessKeyId.Transform(input);
            AwsSecretAccessKey = RegexPatterns.SecretAccessKey.Transform(input);
            AwsSessionToken = RegexPatterns.SessionToken.Transform(input);
            return this;
        }

        private string FromKeyLinesGet(List<string[]> lines, string key)
            => lines.FirstOrDefault(l => l[0].Trim().ToLower() == key)?[1].Trim();

        public bool IsDefault() => UserName == DefaultProfileName;

        public bool IsValid() => !string.IsNullOrWhiteSpace(UserName)
                                 && !string.IsNullOrWhiteSpace(Region)
                                 && !string.IsNullOrWhiteSpace(AwsAccessKeyId)
                                 && !string.IsNullOrWhiteSpace(AwsSecretAccessKey)
                                 && !string.IsNullOrWhiteSpace(AwsSessionToken);

        public bool MightFail() => false; // AwsAccessKeyId.Contains('+')||AwsSecretAccessKey.Contains('+'); // right now there seems to be no known critical case

        public override string ToString()
        {
            return ToString(null);
        }

        public string ToString(string defaultRegion)
        {
            StringBuilder sb = new();
            sb.AppendLine($"[{UserName}]");
            sb.AppendLine($"aws_access_key_id = {AwsAccessKeyId}");
            sb.AppendLine($"aws_secret_access_key = {AwsSecretAccessKey}");
            sb.AppendLine($"aws_session_token = {AwsSessionToken}");
            var region = Region ?? defaultRegion;
            if (region != null)
            {
                sb.AppendLine($"region = {region}");
            }
            return sb.ToString();
        }
    }
}