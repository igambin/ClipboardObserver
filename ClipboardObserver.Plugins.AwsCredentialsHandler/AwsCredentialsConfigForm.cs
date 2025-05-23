﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClipboardObserver.Common;
using ClipboardObserver.PluginManagement;
using Microsoft.Extensions.DependencyInjection;

namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    public partial class AwsCredentialsConfigForm : PluginConfigForm
    {


        public AwsCredentialsConfigForm() : base()
        {
            InitForm();
            this.Shown += AwsCredentialsConfigForm_Shown1;
        }

        protected void InitForm()
        {
            InitializeComponent();
            base.InitForm<AwsCredentialsConfigOptions>(GlobalInstances.ServiceProvider);
            tbRegion.DataBindings.Add("Text", Options, "DefaultRegion");
            cbToEnv.DataBindings.Add("Checked", Options, "ExportCredentialsToEnv");
            cbToFile.DataBindings.Add("Checked", Options, "StoreCredentialsInFile");
            tbFileName.DataBindings.Add("Text", Options, "AwsCredentialsFile");
            cbCloneDefault.DataBindings.Add("Checked", Options, "CloneCredentialsToDefault");
            cbDefaultOnly.DataBindings.Add("Checked", Options, "WriteDefaultProfileOnly", true, DataSourceUpdateMode.OnPropertyChanged);
            cbAddRegion.DataBindings.Add("Checked", Options, "AddRegionToCredentialsFile");
            cbWriteConfig.DataBindings.Add("Checked", Options, "WriteRegionToConfigFile");
            tbCfgFileName.DataBindings.Add("Text", Options, "AwsCredentialsConfigFile");
        }

        private void AwsCredentialsConfigForm_Shown1(object sender, EventArgs e)
        {
            CbDefaultOnly_CheckedChanged(cbDefaultOnly, EventArgs.Empty);
        }
        
        private void CbToFile_CheckedChanged(object sender, EventArgs e)
        {
            cbCloneDefault.Enabled = !cbCloneDefault.Checked;
            cbAddRegion.Enabled = cbToFile.Checked;
        }

        private void CbDefaultOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDefaultOnly.Checked)
            {
                cbCloneDefault.Checked = true;
            }
            cbCloneDefault.Enabled = !cbDefaultOnly.Checked;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            CancelConfig();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveConfig<AwsCredentialsConfigOptions>();
        }

        private void AwsCredentialsConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void unsetEnvVariablesButton_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null, EnvironmentVariableTarget.User);
                Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null, EnvironmentVariableTarget.User);
                Environment.SetEnvironmentVariable("AWS_SESSION_TOKEN", null, EnvironmentVariableTarget.User);
                MessageBox.Show("Deletion of environment variables done.");
            });
            MessageBox.Show(
                "Programmatic removal of these environment variables will take ~30 seconds. Please don't close this app before.");
        }

    }
}
