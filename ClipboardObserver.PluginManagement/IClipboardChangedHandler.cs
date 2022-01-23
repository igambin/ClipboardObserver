using System.Collections.Generic;
using System.Threading.Tasks;
using WK.Libraries.SharpClipboardNS;

namespace ClipboardObserver.PluginManagement
{
    public interface IClipboardChangedHandler
    {
        event ClipboardEntryProcessedEventHandler ClipboardEntryProcessed;

        List<SharpClipboard.ContentTypes> TriggeredBy { get; }

        Task ClipboardChanged();
        
        void OnClipboardProcessed(string message, ClipboardProcessingEventSeverity severity = ClipboardProcessingEventSeverity.Info);

        bool IsActive { get; set; }
    }
}
