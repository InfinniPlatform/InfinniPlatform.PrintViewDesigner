using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Block
{
    internal class PrintPageBreakBreakEditorFactory : EditorFactoryBase<PrintPageBreak>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddCommonEditors(editor);

            return editor;
        }
    }
}