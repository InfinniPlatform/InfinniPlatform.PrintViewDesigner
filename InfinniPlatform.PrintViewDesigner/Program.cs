using System;
using System.Windows;

using InfinniPlatform.PrintViewDesigner.Views;

namespace InfinniPlatform.PrintViewDesigner
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            new Application { MainWindow = new MainWindow { Visibility = Visibility.Visible } }.Run();
        }
    }
}