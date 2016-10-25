using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal class PrintStyleEditorFactory : EditorFactoryBase<PrintStyle>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddCommonEditors(editor);

            // Text
            var textCategory = Builder.AddTextCategory(editor);
            Builder.AddEditor(textCategory, Resources.FontProperty, i => i.Font);
            Builder.AddTextCaseEditor(textCategory, i => i.TextCase);
            Builder.AddTextDecorationEditor(textCategory, i => i.TextDecoration);
            Builder.AddTextAlignmentEditor(textCategory, i => i.TextAlignment);

            // Layout
            var layoutCategory = Builder.AddLayoutCategory(editor);
            Builder.AddEditor(layoutCategory, Resources.MarginProperty, i => i.Margin);
            Builder.AddEditor(layoutCategory, Resources.PaddingProperty, i => i.Padding);

            // Appearance
            var appearanceCategory = Builder.AddAppearanceCategory(editor);
            Builder.AddColorEditor(appearanceCategory, Resources.ForegroundProperty, i => i.Foreground);
            Builder.AddColorEditor(appearanceCategory, Resources.BackgroundProperty, i => i.Background);
            Builder.AddEditor(appearanceCategory, Resources.BorderProperty, i => i.Border);

            return editor;
        }
    }
}