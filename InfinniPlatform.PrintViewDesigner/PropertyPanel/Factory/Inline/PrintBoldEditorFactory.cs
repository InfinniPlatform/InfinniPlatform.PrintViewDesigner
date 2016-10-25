using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Inline
{
    internal class PrintBoldEditorFactory : EditorFactoryBase<PrintBold>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddInlineEditors(editor);

            return editor;
        }
    }
}