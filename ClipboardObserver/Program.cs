using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClipboardObserver.PluginManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WK.Libraries.SharpClipboardNS;

namespace ClipboardObserver
{
    static class Program
    {
        public static IConfiguration Configuration { get; private set; }

        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Startup(new ServiceCollection());
            
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = ServiceProvider.GetService<ClipboardObserverForm>();
            Application.Run(form);
        }

        private static void Startup(IServiceCollection services)
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


            var pm = new PluginManager();
            var optionSetters = pm.Startup(services, configBuilder);
            
            Configuration = configBuilder.Build();

            optionSetters.ForEach(o => o.Invoke(services, Configuration));

            services.AddSingleton(Configuration);

            ServiceProvider = services.BuildServiceProvider();


        }



    }
}
