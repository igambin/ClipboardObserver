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
        private IConfiguration Config { get; set; }
        private string OptionsName { get; set; }
        protected object Options { get; set; }

        public PluginConfigForm()
        {
        }

        protected virtual void InitForm<TForm>(IServiceProvider provider)
        {
            Config = provider.GetRequiredService<IConfiguration>();
            var monitor = provider.GetRequiredService<IOptionsMonitor<TForm>>();
            OptionsName = typeof(TForm).Name;
            Options = monitor.CurrentValue;
        }

        protected void CancelConfig()
        {
            Hide();
        }

        protected void SaveConfig()
        {
            var configRoot = (IConfigurationRoot)Config;

            var configProvider =
                configRoot
                    .Providers
                    .OfType<JsonConfigurationProvider>()
                    .FirstOrDefault(rp => rp.Source.Path.ToLower() == $"{OptionsName}.json".ToLower());

            if (configProvider?.Source.FileProvider is PhysicalFileProvider fileProvider)
            {
                var path = Path.Combine(fileProvider.Root, $"{OptionsName}.json");
                var options = new ExpandoObject();
                if (options.TryAdd(OptionsName, Options))
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
        }

        //private void PluginConfigForm_Shown(object sender, System.EventArgs e)
        //{
        //    InitForm();
        //}
    }
}