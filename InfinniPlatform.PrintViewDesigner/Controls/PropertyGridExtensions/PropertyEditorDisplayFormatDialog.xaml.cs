using System.Windows;

using DevExpress.Xpf.Core;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorDisplayFormatDialog : DXWindow
    {
        public PropertyEditorDisplayFormatDialog()
        {
            InitializeComponent();
        }

        public object EditValue
        {
            get { return GetEditValue(); }
            set { SetEditValue(value); }
        }

        private dynamic GetEditValue()
        {
            dynamic editValue = null;

            if (BooleanFormatTab.Equals(FormatTabControl.SelectedTabItem))
            {
                editValue = new DynamicWrapper();
                editValue.BooleanFormat = new DynamicWrapper();
                editValue.BooleanFormat.FalseText = BooleanFormatFalseTextEditor.EditValue ?? BooleanFormatFalseTextEditor.NullValue;
                editValue.BooleanFormat.TrueText = BooleanFormatTrueTextEditor.EditValue ?? BooleanFormatTrueTextEditor.NullValue;
            }
            else if (DateTimeFormatTab.Equals(FormatTabControl.SelectedTabItem))
            {
                editValue = new DynamicWrapper();
                editValue.DateTimeFormat = new DynamicWrapper();
                editValue.DateTimeFormat.Format = DateTimeFormatEditor.EditValue ?? DateTimeFormatEditor.NullValue;
            }
            else if (NumberFormatTab.Equals(FormatTabControl.SelectedTabItem))
            {
                editValue = new DynamicWrapper();
                editValue.NumberFormat = new DynamicWrapper();
                editValue.NumberFormat.Format = NumberFormatEditor.EditValue ?? NumberFormatEditor.NullValue;
            }
            else if (ObjectFormatTab.Equals(FormatTabControl.SelectedTabItem))
            {
                editValue = new DynamicWrapper();
                editValue.ObjectFormat = new DynamicWrapper();
                editValue.ObjectFormat.Format = ObjectFormatEditor.EditValue ?? ObjectFormatEditor.NullValue;
            }

            return editValue;
        }

        private void SetEditValue(dynamic editValue)
        {
            if (editValue != null)
            {
                if (editValue.BooleanFormat != null)
                {
                    FormatTabControl.SelectedTabItem = BooleanFormatTab;
                    BooleanFormatFalseTextEditor.EditValue = editValue.BooleanFormat.FalseText;
                    BooleanFormatTrueTextEditor.EditValue = editValue.BooleanFormat.TrueText;
                }
                else if (editValue.DateTimeFormat != null)
                {
                    FormatTabControl.SelectedTabItem = DateTimeFormatTab;
                    DateTimeFormatEditor.EditValue = editValue.DateTimeFormat.Format;
                }
                else if (editValue.NumberFormat != null)
                {
                    FormatTabControl.SelectedTabItem = NumberFormatTab;
                    NumberFormatEditor.EditValue = editValue.NumberFormat.Format;
                }
                else if (editValue.ObjectFormat != null)
                {
                    FormatTabControl.SelectedTabItem = ObjectFormatTab;
                    ObjectFormatEditor.EditValue = editValue.ObjectFormat.Format;
                }
            }
        }

        private void OnAcceptButton(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnCancelButton(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}