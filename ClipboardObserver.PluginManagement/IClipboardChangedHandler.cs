using WK.Libraries.SharpClipboardNS;

namespace ClipboardObserver.PluginManagement
{
    public interface IClipboardChangedHandler
    {
        event ClipboardEntryProcessedEventHandler ClipboardEntryProcessed;

        string Name { get; }
        
        SharpClipboard.ContentTypes ContentType { get; }

        void ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e);
        
        void OnClipboardProcessed(string message);
    }
}
