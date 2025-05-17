using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace ClipboardObserver.PluginManagement
{
    public class PluginConfigForm : Form
    {
        private IServiceProvider ServiceProvider { get; set; }
        private IConfiguration Config { get; set; }
        private object OptionsMonitor { get; set; }
        protected object Options { get; set; }

        public PluginConfigForm()
        {
        }

        protected virtual void InitForm<TOptions>(IServiceProvider provider)
        {
            ServiceProvider = provider;
            Config = ServiceProvider.GetRequiredService<IConfiguration>();
            OptionsMonitor = ServiceProvider.GetRequiredService<IOptionsMonitor<TOptions>>();
            Options = ((IOptionsMonitor<TOptions>)OptionsMonitor).CurrentValue;
        }

        protected void CancelConfig()
        {
            Hide();
        }

        protected void SaveConfig<TOptions>()
        {
            var configRoot = (IConfigurationRoot)Config;

            var optionsName = typeof(TOptions).Name;

            var configProvider =
                configRoot
                    .Providers
                    .OfType<JsonConfigurationProvider>()
                    .FirstOrDefault(rp => rp.Source.Path != null && rp.Source.Path.ToLower().Equals($"{optionsName}.json".ToLower()));

            if (configProvider?.Source.FileProvider is PhysicalFileProvider fileProvider)
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ClipboardObserver", $"{optionsName}.json");
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                var options = new ExpandoObject();
                if (options.TryAdd(optionsName, Options))
                {
                    var obj = JsonSerializer.Serialize(
                        options, 
                        new JsonSerializerOptions { WriteIndented = true }
                    );
                    using var fwr = new StreamWriter(path);
                    fwr.Write(obj);
                    fwr.Flush();
                }
            }
            Hide();
            Application.Restart();
            Environment.Exit(0);
        }

        //private void PluginConfigForm_Shown(object sender, System.EventArgs e)
        //{
        //    InitForm();
        //}
    }
}