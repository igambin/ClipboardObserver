using System;

namespace ClipboardListener.App
{
    public class ClipboardEntryProcessedEventArgs : EventArgs
    {
        public string Handler { get; set; }
        public string Message { get; set; }
    }
}