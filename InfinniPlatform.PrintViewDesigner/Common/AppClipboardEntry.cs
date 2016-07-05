namespace InfinniPlatform.PrintViewDesigner.Common
{
    internal sealed class AppClipboardEntry
    {
        public readonly bool Copy;
        public readonly object Data;

        public AppClipboardEntry(object data, bool copy)
        {
            Data = data;
            Copy = copy;
        }
    }
}