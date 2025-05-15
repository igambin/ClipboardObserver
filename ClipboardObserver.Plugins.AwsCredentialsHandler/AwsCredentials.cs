using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ClipboardObserver.Common;
using Microsoft.Extensions.Options;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public class AwsCredentials(IOptionsMonitor<AwsCredentialsConfigOptions> options)
    {

        public AwsCredentialsConfigOptions Options { get; } = options.CurrentValue;

        private const string DefaultProfileName = "default";

        public sealed class RegexPatterns
        {
            private static readonly Regex UserNamePattern = new(@"\s*\[(?<profilename>[a-zA-Z0-9_-]+)\]\s*");
            private static readonly Regex RegionPattern = new(@"\s*region\s*=\s*(?<regionname>[\w-]+)\s*");
            private static readonly Regex AwsAccessKeyIdPattern = new(@"\s*aws_access_key_id\s*=\s*(?<accesskey>\w+)\s*");
            private static readonly Regex AwsSecretAccessKeyPattern = new(@"\s*aws_secret_access_key\s*=\s*(?<secretkey>[\w/+]+)\s*");
            private static readonly Regex AwsSessionTokenPattern = new(@"\s*aws_session_token\s*=\s*(?<sessiontoken>[\w/+]+)\s*");
            private static readonly Regex SourceProfilePattern = new(@"\s*source_profile\s*=\s*(?<sourceprofile>\w+)\s*");
            private static readonly Regex RoleArnPattern = new(@"\s*role_arn\s*=\s*(?<rolearn>[\w:/_-]+)\s*");
            public static readonly RegExMatcher UserNameMatcher = new(UserNamePattern);
            public static readonly RegExPattern Name = new(UserNamePattern, (groups) => groups["profilename"].ToString());
            public static readonly RegExPattern Region = new(RegionPattern, (groups) => groups["regionname"].ToString());
            public static readonly RegExPattern AccessKeyId = new(AwsAccessKeyIdPattern, (groups) => groups["accesskey"].ToString());
            public static readonly RegExPattern SecretAccessKey = new(AwsSecretAccessKeyPattern, (groups) => groups["secretkey"].ToString());
            public static readonly RegExPattern SessionToken = new(AwsSessionTokenPattern, (groups) => groups["sessiontoken"].ToString());
            public static readonly RegExPattern SourceProfile = new(SourceProfilePattern, (groups) => groups["sourceprofile"].ToString());
            public static readonly RegExPattern RoleArn = new(RoleArnPattern, (groups) => groups["rolearn"].ToString());
        }

        [Description("[{0}]")]
        public string UserName { get; set; }

        [Description("region = {0}")]
        public string Region { get; set; }

        [Description("aws_access_key_id = {0}")]
        public string AwsAccessKeyId { get; set; }

        [Description("aws_secret_access_key = {0}")]
        public string AwsSecretAccessKey { get; set; }

        [Description("aws_session_token = {0}")]
        public string AwsSessionToken { get; set; }

        [Description("source_profile = {0}")]
        public string SourceProfile { get; set; }

        [Description("role_arn = {0}")]
        public string RoleArn { get; set; }

        public AwsCredentials UpdateFromProfile(AwsCredentials profile, string newUserName = null)
        {
            UserName = newUserName ?? profile.UserName;
            Region = profile.Region;
            if (string.IsNullOrWhiteSpace(Region)) Region = Options.DefaultRegion;
            AwsAccessKeyId = profile.AwsAccessKeyId;
            AwsSecretAccessKey = profile.AwsSecretAccessKey;
            AwsSessionToken = profile.AwsSessionToken;
            if (!string.IsNullOrWhiteSpace(profile.SourceProfile))
            {
                SourceProfile = profile.SourceProfile;
            }

            if (!string.IsNullOrWhiteSpace(profile.RoleArn))
            {
                RoleArn = profile.RoleArn;
            }
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
            SourceProfile = RegexPatterns.SourceProfile.Transform(input);
            RoleArn = RegexPatterns.RoleArn.Transform(input);
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

        public override string ToString()
        {
            return ToString(null);
        }

        private void AppendLine(StringBuilder sb, AwsCredentials entity, Expression<Func<AwsCredentials, string>> expression)
        {
            var memberExpression = (MemberExpression)expression.Body;
            var value = expression.Compile()(entity);
            
            if (string.IsNullOrWhiteSpace(value)) return;
            
            var attr = memberExpression.Member.GetCustomAttribute<DescriptionAttribute>();
            var tmpl = attr?.Description ?? $"no template defined for {memberExpression.Member.Name}";
            sb.AppendLine(string.Format(tmpl, value));
        }

        public string ToString(string defaultRegion)
        {
            StringBuilder sb = new();
            AppendLine(sb, this, x => x.UserName);
            AppendLine(sb, this, x => x.AwsAccessKeyId);
            AppendLine(sb, this, x => x.AwsSecretAccessKey);
            AppendLine(sb, this, x => x.AwsSessionToken);
            AppendLine(sb, this, x => x.SourceProfile);
            AppendLine(sb, this, x => x.RoleArn);
            Region ??= defaultRegion;
            AppendLine(sb, this, x => x.Region);

            return sb.ToString();
        }
    }
}