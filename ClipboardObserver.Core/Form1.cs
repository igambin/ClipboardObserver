using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WK.Libraries.SharpClipboardNS;

namespace ClipboardListener.App
{
    public partial class Form1 : Form
    {
        static int CopiedObjectsObserved { get; set; }
        static int CopiedTextItemsObserved { get; set; }
        static int CopiedFilesObserved { get; set; }
        static int CopiedImagesObserved { get; set; }

        private SharpClipboard ClipBoard { get; }

        private IServiceProvider Services { get; }

        public Form1(SharpClipboard clipBoard, IServiceProvider services)
        {
            ClipBoard = clipBoard;
            ClipBoard.ObserveLastEntry = false;

            Services = services;

            InitForm();
            
            ClipBoard.ClipboardChanged += ClipBoard_ClipboardChanged;
            Services.GetServices<IClipboardChangedSubscriber>().ToList().ForEach(x =>
            {
                x.ClipboardEntryProcessed += ClipboardEntryProcessed;
                textBox1.AppendText(x.Name + " subscribed to new clipboard items!"+Environment.NewLine);
            });
            
        }

        public void InitForm()
        {
            InitializeComponent();            
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
            ClipBoard.StopMonitoring();
        }
    }
}
