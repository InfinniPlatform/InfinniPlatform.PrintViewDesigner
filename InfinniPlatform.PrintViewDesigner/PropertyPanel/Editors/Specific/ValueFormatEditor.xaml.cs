using System.Windows;

using InfinniPlatform.PrintView.Model.Format;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific
{
    public partial class ValueFormatEditor : PropertyEditorBase
    {
        public ValueFormatEditor()
        {
            InitializeComponent();
        }

        private void OnShouwEditor(object sender, RoutedEventArgs e)
        {
            var editView = new ValueFormatEditorDialog { EditValue = (ValueFormat)Editor.EditValue };

            if (editView.ShowDialog() == true)
            {
                Editor.EditValue = editView.EditValue;
            }
        }
    }
}