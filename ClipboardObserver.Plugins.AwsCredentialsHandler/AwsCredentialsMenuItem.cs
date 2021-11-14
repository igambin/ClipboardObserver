using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _services.GetService<AwsCredentialsConfigForm>()?.Show();
        }

        public ToolStripMenuItem GetMenuItem() => AwsCredentialsConfigToolstripMenuItem;

    }
}
