using System.Windows;

using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintViewDesigner.MainView
{
    public partial class PrintViewDesignerWindow : Window
    {
        public PrintViewDesignerWindow()
        {
            InitializeComponent();

            Designer.DocumentTemplate = new PrintDocument();
        }
    }
}