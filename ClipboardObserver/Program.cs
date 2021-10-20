using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ClipboardObserver.PluginManagement;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
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

            var services = CreateServiceProvider();
            var form = services.GetRequiredService<Form1>();
            Application.Run(form);
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<Form1>()
                .AddSingleton<SharpClipboard>();

            var pm = new PluginManager();
            pm.RegisterPlugins(services);

            return services.CreateLightInjectServiceProvider();
        }
    }
}
