using ClipboardObserver.PluginManagement;
using WK.Libraries.SharpClipboardNS;

namespace ClipboardObserver.Plugins.TestHandler
{
    public class TestHandler : IClipboardChangedHandler
    {
        public event ClipboardEntryProcessedEventHandler ClipboardEntryProcessed;

        private SharpClipboard Clipboard { get; }
        
        public string Name { get; } = nameof(TestHandler);

        public SharpClipboard.ContentTypes ContentType { get; } = SharpClipboard.ContentTypes.Image;

        public TestHandler(SharpClipboard clipboard)
        {
            Clipboard = clipboard;
            Clipboard.ClipboardChanged += ClipboardChanged;
        }

        public void ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            if (e.ContentType == ContentType)
            {
                OnClipboardProcessed("Clipboard received a copy of an image!");
            }
        }

        public void OnClipboardProcessed(string message)
        {
            if (ClipboardEntryProcessed != null)
            {
                ClipboardEntryProcessed(this, new ClipboardEntryProcessedEventArgs {Handler = Name, Message = message});
            }
        }
    }
}
