using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Block
{
    internal class PrintParagraphEditorFactory : EditorFactoryBase<PrintParagraph>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddBlockEditors(editor);

            var indentSizeEditor = new IndentSizeEditor { Caption = Resources.ParagraphIndent };
            Builder.AddEditor(indentSizeEditor, indentSizeEditor.SizeEditor, i => i.IndentSize);
            Builder.AddEditor(indentSizeEditor, indentSizeEditor.SizeUnitEditor, i => i.IndentSizeUnit);

            var textCategory = Builder.AddTextCategory(editor);
            Builder.AddEditor(textCategory, indentSizeEditor);

            return editor;
        }
    }
}