
namespace ClipboardObserver
{
    partial class ClipboardObserverForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClipboardObserverForm));
            textBox1 = new System.Windows.Forms.TextBox();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            aboutLabel = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            numTexts = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            numImages = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            numFiles = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            numOthers = new System.Windows.Forms.ToolStripStatusLabel();
            ClipboardObserverNotifier = new System.Windows.Forms.NotifyIcon(components);
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            closeObserverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            statusStrip1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox1.Enabled = false;
            textBox1.Location = new System.Drawing.Point(0, -1);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox1.Size = new System.Drawing.Size(585, 137);
            textBox1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutLabel, toolStripStatusLabel1, numTexts, toolStripStatusLabel2, numImages, toolStripStatusLabel4, numFiles, toolStripStatusLabel6, numOthers });
            statusStrip1.Location = new System.Drawing.Point(0, 139);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(584, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.GrayText;
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(183, 17);
            toolStripStatusLabel1.Text = "Copied Texts/Images/Files/Other:";
            // 
            // numTexts
            // 
            numTexts.ForeColor = System.Drawing.SystemColors.GrayText;
            numTexts.Name = "numTexts";
            numTexts.Size = new System.Drawing.Size(13, 17);
            numTexts.Text = "0";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.ForeColor = System.Drawing.SystemColors.GrayText;
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(12, 17);
            toolStripStatusLabel2.Text = "/";
            // 
            // numImages
            // 
            numImages.ForeColor = System.Drawing.SystemColors.GrayText;
            numImages.Name = "numImages";
            numImages.Size = new System.Drawing.Size(13, 17);
            numImages.Text = "0";
            // 
            // toolStripStatusLabel4
            // 
            toolStripStatusLabel4.ForeColor = System.Drawing.SystemColors.GrayText;
            toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            toolStripStatusLabel4.Size = new System.Drawing.Size(12, 17);
            toolStripStatusLabel4.Text = "/";
            // 
            // numFiles
            // 
            numFiles.ForeColor = System.Drawing.SystemColors.GrayText;
            numFiles.Name = "numFiles";
            numFiles.Size = new System.Drawing.Size(13, 17);
            numFiles.Text = "0";
            // 
            // toolStripStatusLabel6
            // 
            toolStripStatusLabel6.ForeColor = System.Drawing.SystemColors.GrayText;
            toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            toolStripStatusLabel6.Size = new System.Drawing.Size(12, 17);
            toolStripStatusLabel6.Text = "/";
            // 
            // numOthers
            // 
            numOthers.ForeColor = System.Drawing.SystemColors.GrayText;
            numOthers.Name = "numOthers";
            numOthers.Size = new System.Drawing.Size(13, 17);
            numOthers.Text = "0";
            // 
            // ClipboardObserverNotifier
            // 
            ClipboardObserverNotifier.BalloonTipTitle = "Clipboard Observer";
            ClipboardObserverNotifier.ContextMenuStrip = contextMenuStrip1;
            ClipboardObserverNotifier.Icon = (System.Drawing.Icon)resources.GetObject("ClipboardObserverNotifier.Icon");
            ClipboardObserverNotifier.Text = "ClipboardObserver";
            ClipboardObserverNotifier.Visible = true;
            ClipboardObserverNotifier.DoubleClick += ClipboardObserverNotifier_MouseDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { closeObserverToolStripMenuItem, toolStripSeparator1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(154, 32);
            // 
            // closeObserverToolStripMenuItem
            // 
            closeObserverToolStripMenuItem.Name = "closeObserverToolStripMenuItem";
            closeObserverToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            closeObserverToolStripMenuItem.Text = "Close Observer";
            closeObserverToolStripMenuItem.Click += CloseObserverToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(150, 6);
            // 
            // ClipboardObserverForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(584, 161);
            Controls.Add(statusStrip1);
            Controls.Add(textBox1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximumSize = new System.Drawing.Size(800, 400);
            MinimumSize = new System.Drawing.Size(600, 200);
            Name = "ClipboardObserverForm";
            Text = "ClipboardObserver";
            FormClosing += Form1_FormClosing;
            Resize += Form1_Resize;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel numTexts;
        private System.Windows.Forms.ToolStripStatusLabel numImages;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel numFiles;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel numOthers;
        private System.Windows.Forms.NotifyIcon ClipboardObserverNotifier;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem closeObserverToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel aboutLabel;
    }
}

