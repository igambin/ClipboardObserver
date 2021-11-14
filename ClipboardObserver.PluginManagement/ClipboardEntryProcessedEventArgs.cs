using System;

namespace ClipboardObserver.PluginManagement
{
    public class ClipboardEntryProcessedEventArgs : EventArgs
    {
        public object Handler { get; set; }
        public string Message { get; set; }
    }
}