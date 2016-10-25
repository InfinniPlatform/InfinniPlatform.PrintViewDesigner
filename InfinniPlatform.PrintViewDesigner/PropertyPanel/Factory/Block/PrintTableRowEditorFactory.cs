using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Block
{
    internal class PrintTableRowEditorFactory : EditorFactoryBase<PrintTableRow>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddCommonEditors(editor);

            var textCategory = Builder.AddTextCategory(editor);
            Builder.AddEditor(textCategory, Resources.FontProperty, i => i.Font);
            Builder.AddTextCaseEditor(textCategory, i => i.TextCase);

            var appearanceCategory = Builder.AddAppearanceCategory(editor);
            Builder.AddStyleNameEditor(appearanceCategory, i => i.Style);
            Builder.AddColorEditor(appearanceCategory, Resources.ForegroundProperty, i => i.Foreground);
            Builder.AddColorEditor(appearanceCategory, Resources.BackgroundProperty, i => i.Background);

            return editor;
        }
    }
}