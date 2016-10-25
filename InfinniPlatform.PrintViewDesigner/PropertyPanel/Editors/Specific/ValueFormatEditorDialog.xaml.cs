using System.Windows;

using DevExpress.Xpf.Core;

using InfinniPlatform.PrintView.Model.Format;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific
{
    public partial class ValueFormatEditorDialog : DXWindow
    {
        public ValueFormatEditorDialog()
        {
            InitializeComponent();
        }

        public ValueFormat EditValue
        {
            get { return GetEditValue(); }
            set { SetEditValue(value); }
        }

        private ValueFormat GetEditValue()
        {
            if (BooleanFormatTab.Equals(FormatTabControl.SelectedTabItem))
            {
                return new BooleanFormat
                       {
                           FalseText = (string)(BooleanFormatFalseTextEditor.EditValue ?? BooleanFormatFalseTextEditor.NullValue),
                           TrueText = (string)(BooleanFormatTrueTextEditor.EditValue ?? BooleanFormatTrueTextEditor.NullValue)
                       };
            }

            if (DateTimeFormatTab.Equals(FormatTabControl.SelectedTabItem))
            {
                return new DateTimeFormat
                       {
                           Format = (string)(DateTimeFormatEditor.EditValue ?? DateTimeFormatEditor.NullValue)
                       };
            }

            if (NumberFormatTab.Equals(FormatTabControl.SelectedTabItem))
            {
                return new NumberFormat
                       {
                           Format = (string)(NumberFormatEditor.EditValue ?? NumberFormatEditor.NullValue)
                       };
            }

            if (ObjectFormatTab.Equals(FormatTabControl.SelectedTabItem))
            {
                return new ObjectFormat
                       {
                           Format = (string)(ObjectFormatEditor.EditValue ?? ObjectFormatEditor.NullValue)
                       };
            }

            return null;
        }

        private void SetEditValue(ValueFormat editValue)
        {
            if (editValue != null)
            {
                if (editValue is BooleanFormat)
                {
                    FormatTabControl.SelectedTabItem = BooleanFormatTab;
                    BooleanFormatFalseTextEditor.EditValue = ((BooleanFormat)editValue).FalseText;
                    BooleanFormatTrueTextEditor.EditValue = ((BooleanFormat)editValue).TrueText;
                }
                else if (editValue is DateTimeFormat)
                {
                    FormatTabControl.SelectedTabItem = DateTimeFormatTab;
                    DateTimeFormatEditor.EditValue = ((DateTimeFormat)editValue).Format;
                }
                else if (editValue is NumberFormat)
                {
                    FormatTabControl.SelectedTabItem = NumberFormatTab;
                    NumberFormatEditor.EditValue = ((NumberFormat)editValue).Format;
                }
                else if (editValue is ObjectFormat)
                {
                    FormatTabControl.SelectedTabItem = ObjectFormatTab;
                    ObjectFormatEditor.EditValue = ((ObjectFormat)editValue).Format;
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