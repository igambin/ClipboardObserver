using System;
using System.IO;
using System.Windows.Forms;
using ClipboardObserver.Common;
using ClipboardObserver.PluginManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WK.Libraries.SharpClipboardNS;

namespace ClipboardObserver
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CreateServiceProvider(new ServiceCollection());
            
            RunObserverForm();
        }

        private static void RunObserverForm()
        {
            var mainForm = GlobalInstances.ServiceProvider.GetService<ClipboardObserverForm>();
            Application.Run(mainForm);
        }

        private static void CreateServiceProvider(IServiceCollection services)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                ;

            services.AddLogging(builder =>
            {
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "YY-MM-DD hh:mm:ss";
                });
            });

            services.AddSingleton<ClipboardObserverForm>()
                    .AddSingleton<ClipboardObserverOptions>()
                    .AddSingleton<SharpClipboard>();


            var optionSetters = PluginManager.Startup(services, configBuilder);

            GlobalInstances.Configuration = configBuilder.Build();

            optionSetters.ForEach(o => o.Invoke(services, GlobalInstances.Configuration));

            services.AddSingleton(GlobalInstances.Configuration);

            GlobalInstances.ServiceProvider = services.BuildServiceProvider();


        }



    }
}
