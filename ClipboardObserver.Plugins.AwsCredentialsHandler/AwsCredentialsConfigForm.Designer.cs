 
namespace ClipboardObserver.Plugins.AwsCredentialsHandler
{
    partial class AwsCredentialsConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tbFileName = new System.Windows.Forms.TextBox();
            cbToFile = new System.Windows.Forms.CheckBox();
            cbCloneDefault = new System.Windows.Forms.CheckBox();
            cbAddRegion = new System.Windows.Forms.CheckBox();
            cbToEnv = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            tbRegion = new System.Windows.Forms.TextBox();
            cbWriteConfig = new System.Windows.Forms.CheckBox();
            tbCfgFileName = new System.Windows.Forms.TextBox();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            unsetEnvVariablesButton = new System.Windows.Forms.Button();
            cbDefaultOnly = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // tbFileName
            // 
            tbFileName.Location = new System.Drawing.Point(35, 151);
            tbFileName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tbFileName.Name = "tbFileName";
            tbFileName.ReadOnly = true;
            tbFileName.Size = new System.Drawing.Size(332, 27);
            tbFileName.TabIndex = 1;
            // 
            // cbToFile
            // 
            cbToFile.AutoSize = true;
            cbToFile.Location = new System.Drawing.Point(14, 117);
            cbToFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cbToFile.Name = "cbToFile";
            cbToFile.Size = new System.Drawing.Size(221, 24);
            cbToFile.TabIndex = 3;
            cbToFile.Text = "Store in AWS credentials file:";
            cbToFile.UseVisualStyleBackColor = true;
            cbToFile.CheckedChanged += CbToFile_CheckedChanged;
            // 
            // cbCloneDefault
            // 
            cbCloneDefault.AutoSize = true;
            cbCloneDefault.Location = new System.Drawing.Point(35, 189);
            cbCloneDefault.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cbCloneDefault.Name = "cbCloneDefault";
            cbCloneDefault.Size = new System.Drawing.Size(260, 24);
            cbCloneDefault.TabIndex = 3;
            cbCloneDefault.Text = "clone credentials to default profile";
            cbCloneDefault.UseVisualStyleBackColor = true;
            // 
            // cbAddRegion
            // 
            cbAddRegion.AutoSize = true;
            cbAddRegion.Location = new System.Drawing.Point(35, 253);
            cbAddRegion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cbAddRegion.Name = "cbAddRegion";
            cbAddRegion.Size = new System.Drawing.Size(223, 24);
            cbAddRegion.TabIndex = 4;
            cbAddRegion.Text = "add region to credentials file";
            cbAddRegion.UseVisualStyleBackColor = true;
            // 
            // cbToEnv
            // 
            cbToEnv.AutoSize = true;
            cbToEnv.Location = new System.Drawing.Point(14, 72);
            cbToEnv.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cbToEnv.Name = "cbToEnv";
            cbToEnv.Size = new System.Drawing.Size(308, 24);
            cbToEnv.TabIndex = 6;
            cbToEnv.Text = "Export Credentials to System Environment";
            cbToEnv.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 24);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(56, 20);
            label1.TabIndex = 7;
            label1.Text = "Region";
            // 
            // tbRegion
            // 
            tbRegion.Location = new System.Drawing.Point(71, 20);
            tbRegion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tbRegion.Name = "tbRegion";
            tbRegion.Size = new System.Drawing.Size(297, 27);
            tbRegion.TabIndex = 8;
            // 
            // cbWriteConfig
            // 
            cbWriteConfig.AutoSize = true;
            cbWriteConfig.Location = new System.Drawing.Point(14, 302);
            cbWriteConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cbWriteConfig.Name = "cbWriteConfig";
            cbWriteConfig.Size = new System.Drawing.Size(214, 24);
            cbWriteConfig.TabIndex = 11;
            cbWriteConfig.Text = "Write Region to Config File:";
            cbWriteConfig.UseVisualStyleBackColor = true;
            // 
            // tbCfgFileName
            // 
            tbCfgFileName.Location = new System.Drawing.Point(35, 335);
            tbCfgFileName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tbCfgFileName.Name = "tbCfgFileName";
            tbCfgFileName.ReadOnly = true;
            tbCfgFileName.Size = new System.Drawing.Size(332, 27);
            tbCfgFileName.TabIndex = 12;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(14, 382);
            button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(86, 31);
            button1.TabIndex = 13;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = true;
            button1.Click += CancelButton_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(282, 382);
            button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(86, 31);
            button2.TabIndex = 14;
            button2.Text = "Save";
            button2.UseVisualStyleBackColor = true;
            button2.Click += SaveButton_Click;
            // 
            // unsetEnvVariablesButton
            // 
            unsetEnvVariablesButton.Location = new System.Drawing.Point(304, 67);
            unsetEnvVariablesButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            unsetEnvVariablesButton.Name = "unsetEnvVariablesButton";
            unsetEnvVariablesButton.Size = new System.Drawing.Size(64, 31);
            unsetEnvVariablesButton.TabIndex = 15;
            unsetEnvVariablesButton.Text = "Unset";
            unsetEnvVariablesButton.UseVisualStyleBackColor = true;
            unsetEnvVariablesButton.Click += unsetEnvVariablesButton_Click;
            // 
            // cbDefaultOnly
            // 
            cbDefaultOnly.AutoSize = true;
            cbDefaultOnly.Location = new System.Drawing.Point(35, 221);
            cbDefaultOnly.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cbDefaultOnly.Name = "cbDefaultOnly";
            cbDefaultOnly.Size = new System.Drawing.Size(184, 24);
            cbDefaultOnly.TabIndex = 16;
            cbDefaultOnly.Text = "use default profile only";
            cbDefaultOnly.UseVisualStyleBackColor = true;
            cbDefaultOnly.CheckedChanged += CbDefaultOnly_CheckedChanged;
            // 
            // AwsCredentialsConfigForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(379, 453);
            Controls.Add(cbDefaultOnly);
            Controls.Add(unsetEnvVariablesButton);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(tbCfgFileName);
            Controls.Add(cbWriteConfig);
            Controls.Add(cbAddRegion);
            Controls.Add(tbRegion);
            Controls.Add(cbCloneDefault);
            Controls.Add(label1);
            Controls.Add(tbFileName);
            Controls.Add(cbToFile);
            Controls.Add(cbToEnv);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(397, 500);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(397, 500);
            Name = "AwsCredentialsConfigForm";
            Text = "AWS Credentials Options";
            FormClosing += AwsCredentialsConfigForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();

        }

        private void AwsCredentialsConfigForm_Shown(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.CheckBox cbToFile;
        private System.Windows.Forms.CheckBox cbCloneDefault;
        private System.Windows.Forms.CheckBox cbAddRegion;
        private System.Windows.Forms.CheckBox cbToEnv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbRegion;
        private System.Windows.Forms.CheckBox cbWriteConfig;
        private System.Windows.Forms.TextBox tbCfgFileName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button unsetEnvVariablesButton;
        private System.Windows.Forms.CheckBox cbDefaultOnly;
    }
}