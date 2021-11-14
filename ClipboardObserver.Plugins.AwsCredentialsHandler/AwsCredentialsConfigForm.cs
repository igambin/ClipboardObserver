using ClipboardObserver.PluginManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Windows.Forms;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public partial class AwsCredentialsConfigForm : PluginConfigForm<AwsCredentialsConfigOptions>
    {
        public AwsCredentialsConfigForm(
            IConfiguration configuration,
            IOptionsMonitor<AwsCredentialsConfigOptions> options) : base(configuration, options)
        {
        }

        protected override void InitForm()
        {
            InitializeComponent();
            tbRegion.DataBindings.Add("Text", Options.CurrentValue, "DefaultRegion");
            cbToEnv.DataBindings.Add("Checked", Options.CurrentValue, "ExportCredentialsToEnv");
            cbToFile.DataBindings.Add("Checked", Options.CurrentValue, "StoreCredentialsInFile");
            tbFileName.DataBindings.Add("Text", Options.CurrentValue, "AwsCredentialsFile");
            cbCloneDefault.DataBindings.Add("Checked", Options.CurrentValue, "CloneCredentialsToDefault");
            cbAddRegion.DataBindings.Add("Checked", Options.CurrentValue, "AddRegionToCredentialsFile");
            cbWriteConfig.DataBindings.Add("Checked", Options.CurrentValue, "WriteRegionToConfigFile");
            tbCfgFileName.DataBindings.Add("Text", Options.CurrentValue, "AwsCredentialsConfigFile");
        }

        private void EvaluateEnabledFileOptions()
        {
            cbCloneDefault.Enabled =
            cbAddRegion.Enabled = cbToFile.Checked;
        }

        private void cbToFile_CheckedChanged(object sender, EventArgs e)
        {
            EvaluateEnabledFileOptions();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            CancelConfig();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void AwsCredentialsConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
