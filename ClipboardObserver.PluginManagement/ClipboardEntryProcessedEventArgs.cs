using System;
using System.Windows.Forms;

namespace ClipboardObserver.PluginManagement
{
    public class ClipboardEntryProcessedEventArgs : EventArgs
    {
        public object Handler { get; set; }
        public string Message { get; set; }
        public ClipboardProcessingEventSeverity Severity { get; set; } = ClipboardProcessingEventSeverity.Info;
    }

    public enum ClipboardProcessingEventSeverity
    {
        Info,
        Warning,
        Error
    }
}