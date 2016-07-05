using System.Windows.Input;

namespace InfinniPlatform.PrintViewDesigner.Controls.Commands
{
    public static class PrintElementCommands
    {
        public static readonly ICommand Cut = new RoutedCommand();
        public static readonly ICommand Copy = new RoutedCommand();
        public static readonly ICommand Paste = new RoutedCommand();
        public static readonly ICommand Delete = new RoutedCommand();
        public static readonly ICommand MoveUp = new RoutedCommand();
        public static readonly ICommand MoveDown = new RoutedCommand();
    }
}