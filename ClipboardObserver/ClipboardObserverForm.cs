using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClipboardObserver.PluginManagement;
using Newtonsoft.Json;
using WK.Libraries.SharpClipboardNS;
using FormWindowState = System.Windows.Forms.FormWindowState;

namespace ClipboardObserver
{
    public partial class ClipboardObserverForm : Form
    {
        static int CopiedObjectsObserved { get; set; }
        static int CopiedTextItemsObserved { get; set; }
        static int CopiedFilesObserved { get; set; }
        static int CopiedImagesObserved { get; set; }

        private bool _shutdown = false;

        private SharpClipboard ClipBoard { get; }

        private Dictionary<string, bool> HandlerActiveStates { get; set; }

        public IEnumerable<IClipboardChangedHandler> ClipboardChangedHandlers { get; }
        public IEnumerable<IPluginMenuItem> MenuItems { get; }

        public ClipboardObserverForm(
            SharpClipboard clipBoard,
            IEnumerable<IClipboardChangedHandler> clipboardChangedHandlers,
            IEnumerable<IPluginMenuItem> menuItems
            )
        {
            ClipBoard = clipBoard;
            ClipboardChangedHandlers = clipboardChangedHandlers;
            MenuItems = menuItems;

            InitForm();

            ClipBoard.ClipboardChanged += ClipboardChanged;
        }

        public void InitForm()
        {
            InitializeComponent();

            GetSettings();

            ClipBoard.ObserveLastEntry = false;
            ClipboardObserverNotifier.Visible = true;

            ClipboardChangedHandlers
                .ToList()
                .ForEach(x =>
                {
                    x.ClipboardEntryProcessed += ClipboardEntryProcessed;
                    if (!HandlerActiveStates.ContainsKey(x.GetType().Name))
                    {
                        HandlerActiveStates.Add(x.GetType().Name, true);
                        x.IsActive = true;
                    }
                    else
                    {
                        x.IsActive = HandlerActiveStates[x.GetType().Name];
                    }
                    x.OnClipboardProcessed($"~ subscribes new {string.Join("/", x.TriggeredBy)} clipboard items and is {(x.IsActive ? "en" : "dis")}abled!");

                });

            var closer = contextMenuStrip1.Items[0];
            var separator = contextMenuStrip1.Items[1];
            contextMenuStrip1.Items.Clear();
            MenuItems.ToList().ForEach(x =>
                {
                    contextMenuStrip1.Items.Add(x.GetMenuItem());
                });
            contextMenuStrip1.Items.Add(toolStripSeparator1);
            contextMenuStrip1.Items.Add(closeObserverToolStripMenuItem);

        }

        private void GetSettings()
        {
            HandlerActiveStates = new Dictionary<string, bool>();

            if (!string.IsNullOrWhiteSpace(Properties.ClipboardObserver.Default.HandlerActiveStates))
            {
                HandlerActiveStates = JsonConvert.DeserializeObject<Dictionary<string, bool>>(Properties.ClipboardObserver.Default.HandlerActiveStates);
            }

            if (Properties.ClipboardObserver.Default.Size.Width == 0 || Properties.ClipboardObserver.Default.Size.Height == 0)
            {
                WindowState = FormWindowState.Normal;
                Location = new System.Drawing.Point(200, 200);
                Size = new System.Drawing.Size(600, 200);
            }
            else
            {
                WindowState = Properties.ClipboardObserver.Default.State;
                Location = Properties.ClipboardObserver.Default.Location;
                Size = Properties.ClipboardObserver.Default.Size;
            }
        }

        private void StoreSettings()
        {
            var states = JsonConvert.SerializeObject(HandlerActiveStates);

            Properties.ClipboardObserver.Default.HandlerActiveStates = states;

            Properties.ClipboardObserver.Default.State = WindowState;

            if (WindowState == FormWindowState.Normal)
            {
                Properties.ClipboardObserver.Default.Location = Location;
                Properties.ClipboardObserver.Default.Size = Size;
            }
            else
            {
                Properties.ClipboardObserver.Default.Location = RestoreBounds.Location;
                Properties.ClipboardObserver.Default.Size = RestoreBounds.Size;
            }

            Properties.ClipboardObserver.Default.Save();
        }

        private Dictionary<ClipboardProcessingEventSeverity, ToolTipIcon> SeverityToIconMap = 
            new Dictionary<ClipboardProcessingEventSeverity, ToolTipIcon> {
                { ClipboardProcessingEventSeverity.Info, ToolTipIcon.Info },
                { ClipboardProcessingEventSeverity.Warning, ToolTipIcon.Warning },
                { ClipboardProcessingEventSeverity.Error, ToolTipIcon.Error },
        };


        private void ClipboardEntryProcessed(object source, ClipboardEntryProcessedEventArgs args)
        {
            textBox1.AppendText($"{args.Handler.GetType().Name}: {args.Message}{Environment.NewLine}");
            ClipboardObserverNotifier.BalloonTipTitle = args.Handler.GetType().Name;
            ClipboardObserverNotifier.BalloonTipIcon = SeverityToIconMap[args.Severity];
            ClipboardObserverNotifier.BalloonTipText = args.Message;
            ClipboardObserverNotifier.ShowBalloonTip(1000);
        }

        private async void ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            var tasks = ClipboardChangedHandlers
                .Where(s => s.TriggeredBy.Contains(e.ContentType))
                .ToList()
                .Select(async s => await s.ClipboardChanged());

            await Task.WhenAll(tasks);

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

            if (_shutdown || e.CloseReason == CloseReason.WindowsShutDown)
            {
                StoreSettings();
                ClipBoard.StopMonitoring();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();
        }

        private void ClipboardObserverNotifier_MouseDoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void CloseObserverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _shutdown = true;
            Close();
        }

        private void aboutLabel_DoubleClick(object sender, EventArgs e)
        {
            AboutBox1 a = new();
            a.Show();

        }

        private void aboutLabel_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new();
            a.Show();
        }
    }
}
