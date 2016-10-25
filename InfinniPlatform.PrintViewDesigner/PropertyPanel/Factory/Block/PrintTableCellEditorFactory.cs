using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Block
{
    internal class PrintTableCellEditorFactory : EditorFactoryBase<PrintTableCell>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddCommonEditors(editor);

            var textCategory = Builder.AddTextCategory(editor);
            Builder.AddEditor(textCategory, Resources.FontProperty, i => i.Font);
            Builder.AddTextCaseEditor(textCategory, i => i.TextCase);
            Builder.AddTextAlignmentEditor(textCategory, i => i.TextAlignment);

            var layoutCategory = Builder.AddLayoutCategory(editor);
            Builder.AddEditor(layoutCategory, Resources.PaddingProperty, i => i.Padding);
            Builder.AddIntegerEditor(layoutCategory, Resources.TableCellColumnSpanProperty, i => i.ColumnSpan, minValue: 1);
            Builder.AddIntegerEditor(layoutCategory, Resources.TableCellRowSpanProperty, i => i.RowSpan, minValue: 1);

            var appearanceCategory = Builder.AddAppearanceCategory(editor);
            Builder.AddStyleNameEditor(appearanceCategory, i => i.Style);
            Builder.AddColorEditor(appearanceCategory, Resources.ForegroundProperty, i => i.Foreground);
            Builder.AddColorEditor(appearanceCategory, Resources.BackgroundProperty, i => i.Background);
            Builder.AddEditor(appearanceCategory, Resources.BorderProperty, i => i.Border);

            return editor;
        }
    }
}