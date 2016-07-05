using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.Common
{
    internal class AppClipboard
    {
        public static readonly AppClipboard Default = new AppClipboard();

        private readonly Dictionary<string, AppClipboardEntry> _buffer
            = new Dictionary<string, AppClipboardEntry>();

        public bool ContainsData<T>()
        {
            return _buffer.ContainsKey(GetEntryKey<T>());
        }

        public AppClipboardEntry GetData<T>()
        {
            AppClipboardEntry entry;

            _buffer.TryGetValue(GetEntryKey<T>(), out entry);

            return entry;
        }

        public void SetData<T>(T data, bool copy)
        {
            if (!ReferenceEquals(data, null))
            {
                _buffer[GetEntryKey<T>()] = new AppClipboardEntry(data, copy);
            }
        }

        private static string GetEntryKey<T>()
        {
            return typeof (T).FullName;
        }
    }
}