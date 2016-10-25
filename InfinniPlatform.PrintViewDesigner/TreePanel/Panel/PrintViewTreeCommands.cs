using System.Windows.Input;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Panel
{
    public static class PrintViewTreeCommands
    {
        public static readonly ICommand Cut = new RoutedCommand();
        public static readonly ICommand Copy = new RoutedCommand();
        public static readonly ICommand Paste = new RoutedCommand();
        public static readonly ICommand Delete = new RoutedCommand();
        public static readonly ICommand MoveUp = new RoutedCommand();
        public static readonly ICommand MoveDown = new RoutedCommand();
    }
}