using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ClipboardListener.App.Subscribers;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WK.Libraries.SharpClipboardNS;

namespace ClipboardListener.App
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


            var services = CreateServiceProvider();
            var form = services.GetRequiredService<Form1>();
            Application.Run(form);
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<Form1>()
                .AddSingleton<SharpClipboard>()
                .AddTransient<IClipboardChangedSubscriber, AwsCredentialSubscriber>();

            return services.CreateLightInjectServiceProvider();
        }
    }
}
