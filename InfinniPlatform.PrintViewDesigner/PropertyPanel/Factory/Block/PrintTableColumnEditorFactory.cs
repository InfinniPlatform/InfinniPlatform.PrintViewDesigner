using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Block
{
    internal class PrintTableColumnEditorFactory : EditorFactoryBase<PrintTableColumn>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddCommonEditors(editor);

            var columnSizeEditor = new ColumnSizeEditor { Caption = Resources.ColumnSizeProperty };
            Builder.AddEditor(columnSizeEditor, columnSizeEditor.SizeEditor, i => i.Size);
            Builder.AddEditor(columnSizeEditor, columnSizeEditor.SizeUnitEditor, i => i.SizeUnit);

            var layoutCategory = Builder.AddLayoutCategory(editor);
            Builder.AddEditor(layoutCategory, columnSizeEditor);

            return editor;
        }
    }
}