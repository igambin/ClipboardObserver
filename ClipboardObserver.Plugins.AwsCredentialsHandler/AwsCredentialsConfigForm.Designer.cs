
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
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.cbToFile = new System.Windows.Forms.CheckBox();
            this.cbCloneDefault = new System.Windows.Forms.CheckBox();
            this.cbAddRegion = new System.Windows.Forms.CheckBox();
            this.cbToEnv = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbRegion = new System.Windows.Forms.TextBox();
            this.cbWriteConfig = new System.Windows.Forms.CheckBox();
            this.tbCfgFileName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(31, 113);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.ReadOnly = true;
            this.tbFileName.Size = new System.Drawing.Size(231, 23);
            this.tbFileName.TabIndex = 1;
            // 
            // cbToFile
            // 
            this.cbToFile.AutoSize = true;
            this.cbToFile.Location = new System.Drawing.Point(12, 88);
            this.cbToFile.Name = "cbToFile";
            this.cbToFile.Size = new System.Drawing.Size(175, 19);
            this.cbToFile.TabIndex = 3;
            this.cbToFile.Text = "Store in AWS credentials file:";
            this.cbToFile.UseVisualStyleBackColor = true;
            this.cbToFile.CheckedChanged += new System.EventHandler(this.cbToFile_CheckedChanged);
            // 
            // cbCloneDefault
            // 
            this.cbCloneDefault.AutoSize = true;
            this.cbCloneDefault.Location = new System.Drawing.Point(31, 142);
            this.cbCloneDefault.Name = "cbCloneDefault";
            this.cbCloneDefault.Size = new System.Drawing.Size(206, 19);
            this.cbCloneDefault.TabIndex = 3;
            this.cbCloneDefault.Text = "clone credentials to default profile";
            this.cbCloneDefault.UseVisualStyleBackColor = true;
            // 
            // cbAddRegion
            // 
            this.cbAddRegion.AutoSize = true;
            this.cbAddRegion.Location = new System.Drawing.Point(31, 167);
            this.cbAddRegion.Name = "cbAddRegion";
            this.cbAddRegion.Size = new System.Drawing.Size(176, 19);
            this.cbAddRegion.TabIndex = 4;
            this.cbAddRegion.Text = "add region to credentials file";
            this.cbAddRegion.UseVisualStyleBackColor = true;
            // 
            // cbToEnv
            // 
            this.cbToEnv.AutoSize = true;
            this.cbToEnv.Location = new System.Drawing.Point(12, 54);
            this.cbToEnv.Name = "cbToEnv";
            this.cbToEnv.Size = new System.Drawing.Size(248, 19);
            this.cbToEnv.TabIndex = 6;
            this.cbToEnv.Text = "Export Credentials to System Environment";
            this.cbToEnv.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Region";
            // 
            // tbRegion
            // 
            this.tbRegion.Location = new System.Drawing.Point(62, 15);
            this.tbRegion.Name = "tbRegion";
            this.tbRegion.Size = new System.Drawing.Size(200, 23);
            this.tbRegion.TabIndex = 8;
            // 
            // cbWriteConfig
            // 
            this.cbWriteConfig.AutoSize = true;
            this.cbWriteConfig.Location = new System.Drawing.Point(12, 201);
            this.cbWriteConfig.Name = "cbWriteConfig";
            this.cbWriteConfig.Size = new System.Drawing.Size(171, 19);
            this.cbWriteConfig.TabIndex = 11;
            this.cbWriteConfig.Text = "Write Region to Config File:";
            this.cbWriteConfig.UseVisualStyleBackColor = true;
            // 
            // tbCfgFileName
            // 
            this.tbCfgFileName.Location = new System.Drawing.Point(31, 226);
            this.tbCfgFileName.Name = "tbCfgFileName";
            this.tbCfgFileName.ReadOnly = true;
            this.tbCfgFileName.Size = new System.Drawing.Size(231, 23);
            this.tbCfgFileName.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 261);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(186, 261);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // AwsCredentialsConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 296);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbCfgFileName);
            this.Controls.Add(this.cbWriteConfig);
            this.Controls.Add(this.cbAddRegion);
            this.Controls.Add(this.tbRegion);
            this.Controls.Add(this.cbCloneDefault);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFileName);
            this.Controls.Add(this.cbToFile);
            this.Controls.Add(this.cbToEnv);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(290, 335);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(290, 335);
            this.Name = "AwsCredentialsConfigForm";
            this.Text = "AWS Credentials Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AwsCredentialsConfigForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}