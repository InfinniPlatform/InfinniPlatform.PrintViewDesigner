using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Inline
{
    internal class PrintBarcodeEan13EditorFactory : EditorFactoryBase<PrintBarcodeEan13>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddCommonEditors(editor);

            var dataCategory = Builder.AddDataCategory(editor);
            Builder.AddValueFormatEditor(dataCategory, i => i.SourceFormat);
            Builder.AddStringEditor(dataCategory, Resources.TextProperty, i => i.Text, @"[0-9]+");
            Builder.AddBooleanEditor(dataCategory, Resources.ShowTextProperty, i => i.ShowText);

            var barcodeCategory = Builder.AddCategory(editor, Resources.BarcodeCategory);
            Builder.AddBooleanEditor(dataCategory, Resources.BarcodeEan13CalcCheckSumProperty, i => i.CalcCheckSum);
            Builder.AddDoubleEditor(dataCategory, Resources.BarcodeEan13WideBarRatioProperty, i => i.WideBarRatio);
            Builder.AddImageRotation(barcodeCategory, i => i.Rotation);

            return editor;
        }
    }
}