﻿using System;
using System.IO;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public class AwsCredentialsConfigOptions : IConfigureOptions<AwsCredentialsConfigOptions>
    {
        public AwsCredentialsConfigOptions() { }

        public string DefaultRegion { get; set; }

        public bool ExportCredentialsToEnv { get; set; }

        public bool StoreCredentialsInFile { get; set; }

        public bool CloneCredentialsToDefault { get; set; }

        public bool AddRegionToCredentialsFile { get; set; }

        public bool WriteRegionToConfigFile { get; set; }

        [JsonIgnore]
        public string AwsCredentialsFullPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".aws");

        [JsonIgnore] // called by DataMember-MagicString => 1 reference
        public string AwsCredentialsFile =>  Path.Combine(AwsCredentialsFullPath, "credentials");

        [JsonIgnore] // called by DataMember-MagicString => 1 reference
        public string AwsCredentialsConfigFile => Path.Combine(AwsCredentialsFullPath, "config");

        public void Configure(AwsCredentialsConfigOptions options)
        {
            DefaultRegion = options?.DefaultRegion ?? "eu-central-1";
            ExportCredentialsToEnv = options?.ExportCredentialsToEnv ?? false;
            StoreCredentialsInFile = options?.StoreCredentialsInFile ?? true;
            CloneCredentialsToDefault = options?.CloneCredentialsToDefault ?? true;
            AddRegionToCredentialsFile = options?.AddRegionToCredentialsFile ?? false;
            WriteRegionToConfigFile = options?.WriteRegionToConfigFile ?? true;
        }
    }
}