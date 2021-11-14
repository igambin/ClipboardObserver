using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Win32.SafeHandles;

namespace ClipboardObserver.PluginManagement
{
    public abstract class PluginConfigForm<TOptions> : Form
    {
        private readonly IConfiguration _config;

        protected string OptionsName { get; }

        protected IOptionsMonitor<TOptions> Options { get; private set; }

        public PluginConfigForm(IConfiguration config, IOptionsMonitor<TOptions> options)
        {
            _config = config;
            OptionsName = typeof(TOptions).Name;
            Options = options;
            InitForm();
        }

        protected abstract void InitForm();

        protected void CancelConfig()
        {
            Hide();
        }

        protected void SaveConfig()
        {
            var configRoot = (IConfigurationRoot)_config;

            var configProvider =
                configRoot
                    .Providers
                    .OfType<JsonConfigurationProvider>()
                    .FirstOrDefault(rp => rp.Source.Path.ToLower() == $"{OptionsName}.json".ToLower());

            if (configProvider?.Source.FileProvider is PhysicalFileProvider fileProvider)
            {
                var path = Path.Combine(fileProvider.Root, $"{OptionsName}.json");
                var options = new ExpandoObject();
                if (options.TryAdd(OptionsName, Options.CurrentValue))
                {
                    var obj = JsonSerializer.Serialize(options, new JsonSerializerOptions { WriteIndented = true });
                    using (var fwr = new StreamWriter(path))
                    {
                        fwr.Write(obj);
                        fwr.Flush();
                    }
                }
            }
            Hide();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // PluginConfigForm
            // 
            ClientSize = new System.Drawing.Size(284, 261);
            Name = "PluginConfigForm";
            Shown += new System.EventHandler(PluginConfigForm_Shown);
            ResumeLayout(false);

        }

        private void PluginConfigForm_Shown(object sender, System.EventArgs e)
        {
            InitForm();
        }
    }
}