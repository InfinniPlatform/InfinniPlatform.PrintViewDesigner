using System.Windows;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorDisplayFormat : PropertyEditorBase
    {
        public PropertyEditorDisplayFormat()
        {
            InitializeComponent();
        }

        private void OnShouwEditor(object sender, RoutedEventArgs e)
        {
            var editView = new PropertyEditorDisplayFormatDialog { EditValue = Editor.EditValue };

            if (editView.ShowDialog() == true)
            {
                Editor.EditValue = editView.EditValue;
            }
        }
    }
}