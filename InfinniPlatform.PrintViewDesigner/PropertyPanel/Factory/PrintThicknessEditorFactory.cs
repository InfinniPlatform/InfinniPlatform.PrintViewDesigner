using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal class PrintThicknessEditorFactory : EditorFactoryBase<PrintThickness>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new ThicknessEditor { Caption = caption };

            Builder.AddEditor(editor, editor.TopEditor, i => i.Top);
            Builder.AddEditor(editor, editor.BottomEditor, i => i.Bottom);
            Builder.AddEditor(editor, editor.LeftEditor, i => i.Left);
            Builder.AddEditor(editor, editor.RightEditor, i => i.Right);
            Builder.AddEditor(editor, editor.SizeUnitEditor, i => i.SizeUnit);

            return editor;
        }
    }
}