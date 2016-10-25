using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal class PrintFontEditorFactory : EditorFactoryBase<PrintFont>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new FontEditor { Caption = caption };

            Builder.AddEditor(editor, editor.FontFamilyEditor, i => i.Family);
            Builder.AddEditor(editor, editor.FontSizeEditor, i => i.Size);
            Builder.AddEditor(editor, editor.FontSizeUnitEditor, i => i.SizeUnit);

            Builder.AddEnumEditor(editor, Resources.FontStyleProperty, i => i.Style);
            Builder.AddEnumEditor(editor, Resources.FontStretchProperty, i => i.Stretch);
            Builder.AddEnumEditor(editor, Resources.FontWeightProperty, i => i.Weight);
            Builder.AddEnumEditor(editor, Resources.FontVariantProperty, i => i.Variant);

            return editor;
        }
    }
}