using System;
using ClipboardObserver.PluginManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public class AwsCCredentialsPluginStartup : IPluginStartup
    {
        public Action<IServiceCollection, IConfiguration> Register(IServiceCollection services, IConfigurationBuilder configBuilder)
        {
            services.AddTransient<AwsCredentialsConfigOptions>();
            services.AddTransient<AwsCredentialsConfigForm>();
            services.AddTransient<IPluginMenuItem, AwsCredentialsMenuItem>();
            services.AddSingleton<IClipboardChangedHandler, AwsCredentialsHandler>();

            configBuilder.AddJsonFile($"{nameof(AwsCredentialsConfigOptions)}.json", reloadOnChange: true, optional: false);

            return ConfigureOptions;
        }

        public void ConfigureOptions(IServiceCollection services, IConfiguration config)
        {
            services.Configure<AwsCredentialsConfigOptions>(config.GetSection(nameof(AwsCredentialsConfigOptions)));
        }

    }

}
