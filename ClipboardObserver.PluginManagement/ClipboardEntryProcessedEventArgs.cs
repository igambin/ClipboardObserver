using System;

namespace ClipboardObserver.PluginManagement
{
    public class ClipboardEntryProcessedEventArgs : EventArgs
    {
        public string Handler { get; set; }
        public string Message { get; set; }
    }
}