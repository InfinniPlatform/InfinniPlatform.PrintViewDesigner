using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Block
{
    internal class PrintLineEditorFactory : EditorFactoryBase<PrintLine>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddBlockEditors(editor);

            return editor;
        }
    }
}