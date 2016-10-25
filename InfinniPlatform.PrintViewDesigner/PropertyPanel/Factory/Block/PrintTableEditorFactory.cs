using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Block
{
    internal class PrintTableEditorFactory : EditorFactoryBase<PrintTable>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddBlockEditors(editor);

            var tableCategory = Builder.AddCategory(editor, Resources.TableCategory);
            Builder.AddBooleanEditor(tableCategory, Resources.ShowHeaderProperty, i => i.ShowHeader);

            return editor;
        }
    }
}