using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Inline
{
    internal class PrintLineBreakEditorFactory : EditorFactoryBase<PrintLineBreak>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddCommonEditors(editor);

            return editor;
        }
    }
}