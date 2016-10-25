using System.Windows;

using InfinniPlatform.PrintViewDesigner.Properties;

namespace InfinniPlatform.PrintViewDesigner.Common
{
    internal static class MessageBoxHelpers
    {
        public static void ShowWarningMessage(string message, params object[] args)
        {
            MessageBox.Show(string.Format(message, args),
                            Resources.PrintViewDesignerName,
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
        }

        public static bool AcceptQuestionMessage(string message, params object[] args)
        {
            return MessageBox.Show(string.Format(message, args),
                                   Resources.PrintViewDesignerName,
                                   MessageBoxButton.YesNo,
                                   MessageBoxImage.Warning,
                                   MessageBoxResult.No) == MessageBoxResult.Yes;
        }
    }
}