using System;
using System.IO;
using ClipboardObserver.PluginManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public sealed class AwsCCredentialsPluginStartup : IPluginStartup
    {
        public Action<IServiceCollection, IConfiguration> Register(IServiceCollection services, IConfigurationBuilder configBuilder)
        {
            services
                .AddSingleton<AwsCredentialsConfigForm>()
                .AddTransient<AwsCredentialsFile>()
                .AddTransient<AwsCredentials>()
                .AddTransient<IPluginMenuItem, AwsCredentialsMenuItem>()
                .AddTransient<IClipboardChangedHandler, AwsCredentialsHandler>()
                .AddOptions<AwsCredentialsConfigOptions>();

            var optionsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "ClipboardObserver", 
                $"{nameof(AwsCredentialsConfigOptions)}.json"
                );

            configBuilder.AddJsonFile(optionsPath, reloadOnChange: true, optional: false);

            return ConfigureOptions;
        }

        public void ConfigureOptions(IServiceCollection services, IConfiguration config)
        {
            services.Configure<AwsCredentialsConfigOptions>(config.GetSection(nameof(AwsCredentialsConfigOptions)));
        }

    }

}
