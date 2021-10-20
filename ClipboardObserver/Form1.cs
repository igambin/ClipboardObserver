using System;
using System.Linq;
using System.Windows.Forms;
using ClipboardObserver.PluginManagement;
using Microsoft.Extensions.DependencyInjection;
using WK.Libraries.SharpClipboardNS;
using FormWindowState = System.Windows.Forms.FormWindowState;

namespace ClipboardObserver
{
    public partial class Form1 : Form
    {
        static int CopiedObjectsObserved { get; set; }
        static int CopiedTextItemsObserved { get; set; }
        static int CopiedFilesObserved { get; set; }
        static int CopiedImagesObserved { get; set; }

        private bool _shutdown = false;

        private SharpClipboard ClipBoard { get; }

        private IServiceProvider Services { get; }

        public Form1(SharpClipboard clipBoard, IServiceProvider services)
        {
            ClipBoard = clipBoard;
            Services = services;

            InitForm();
            
            ClipBoard.ClipboardChanged += ClipBoard_ClipboardChanged;
            Services.GetServices<IClipboardChangedHandler>().ToList().ForEach(x =>
            {
                x.ClipboardEntryProcessed += ClipboardEntryProcessed;
                textBox1.AppendText(x.Name + " subscribed to new clipboard items!"+Environment.NewLine);
            });
            
        }

        public void InitForm()
        {
            InitializeComponent();
            ClipBoard.ObserveLastEntry = false;
            ClipboardObserverNotifier.Visible = true;

        }

        private void ClipboardEntryProcessed(object source, ClipboardEntryProcessedEventArgs args)
        {
            textBox1.AppendText(args.Handler + ": " + args.Message+Environment.NewLine);
        }

        private void ClipBoard_ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            switch (e.ContentType)
            {
                case SharpClipboard.ContentTypes.Text:
                    numTexts.Text = @$"{++CopiedTextItemsObserved}";
                    break;
                case SharpClipboard.ContentTypes.Image:
                    numImages.Text = $@"{++CopiedImagesObserved}";
                    break;
                case SharpClipboard.ContentTypes.Files:
                    CopiedFilesObserved += ClipBoard.ClipboardFiles.Count;
                    numFiles.Text = @$"{CopiedFilesObserved}";
                    break; 
                default:
                    numOthers.Text = $@"{++CopiedObjectsObserved}";
                    break;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            if(!_shutdown) e.Cancel = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Minimized) 
                Hide();
        }

        private void ClipboardObserverNotifier_MouseDoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void closeObserverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClipBoard.StopMonitoring();
            _shutdown = true;
            Application.Exit();
        }
    }
}
