using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal class PrintBorderEditorFactory : EditorFactoryBase<PrintBorder>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditorBase { Caption = caption };

            Builder.AddEditor(editor, Resources.BorderThicknessProperty, i => i.Thickness);
            Builder.AddColorEditor(editor, Resources.BorderColorProperty, i => i.Color);

            return editor;
        }
    }
}