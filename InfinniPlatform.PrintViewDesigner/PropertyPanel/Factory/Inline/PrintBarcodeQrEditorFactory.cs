using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Inline
{
    internal class PrintBarcodeQrEditorFactory : EditorFactoryBase<PrintBarcodeQr>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddCommonEditors(editor);

            var dataCategory = Builder.AddDataCategory(editor);
            Builder.AddValueFormatEditor(dataCategory, i => i.SourceFormat);
            Builder.AddStringEditor(dataCategory, Resources.TextProperty, i => i.Text);
            Builder.AddBooleanEditor(dataCategory, Resources.ShowTextProperty, i => i.ShowText);

            var barcodeCategory = Builder.AddCategory(editor, Resources.BarcodeCategory);
            Builder.AddEnumEditor(dataCategory, Resources.BarcodeQrErrorCorrectionProperty, i => i.ErrorCorrection);
            Builder.AddImageRotation(barcodeCategory, i => i.Rotation);

            return editor;
        }
    }
}