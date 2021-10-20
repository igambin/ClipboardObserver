using WK.Libraries.SharpClipboardNS;

namespace ClipboardListener.App
{
    public interface IClipboardChangedSubscriber
    {
        event ClipboardEntryProcessedEventHandler ClipboardEntryProcessed;

        string Name { get; }
        
        SharpClipboard.ContentTypes ContentType { get; }

        void ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e);
        
        void OnClipboardProcessed(string message);
    }
}
