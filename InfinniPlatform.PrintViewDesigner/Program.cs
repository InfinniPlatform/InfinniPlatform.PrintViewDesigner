using System;
using System.Windows;

using InfinniPlatform.PrintViewDesigner.MainView;

namespace InfinniPlatform.PrintViewDesigner
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            new Application { MainWindow = new PrintViewDesignerWindow { Visibility = Visibility.Visible } }.Run();
        }
    }
}