using System;
using System.Windows.Forms;
using ClipboardObserver.PluginManagement;
using Microsoft.Extensions.DependencyInjection;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public partial class AwsCredentialsMenuItem : UserControl, IPluginMenuItem
    {
        public string HandlerName => nameof(AwsCredentialsHandler);

        private readonly IServiceProvider _services;

        public AwsCredentialsMenuItem(IServiceProvider services)
        {
            _services = services;
            InitControl();
        }

        void InitControl()
        {
            InitializeComponent();
        }

        private void ConfigureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _services.GetService<AwsCredentialsConfigForm>()?.Show();
        }

        public ToolStripMenuItem GetMenuItem() => AwsCredentialsConfigToolstripMenuItem;

    }
}
