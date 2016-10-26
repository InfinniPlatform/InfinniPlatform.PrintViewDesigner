using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal class PrintSizeEditorFactory : EditorFactoryBase<PrintSize>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new SizeEditor { Caption = caption };

            Builder.AddClearButton(editor);

            Builder.AddEditor(editor, editor.WidthEditor, i => i.Width);
            Builder.AddEditor(editor, editor.HeightEditor, i => i.Height);
            Builder.AddEditor(editor, editor.SizeUnitEditor, i => i.SizeUnit);

            return editor;
        }
    }
}