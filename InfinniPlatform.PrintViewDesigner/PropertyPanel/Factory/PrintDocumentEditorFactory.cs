using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal class PrintDocumentEditorFactory : EditorFactoryBase<PrintDocument>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddElementEditors(editor);

            var pageCategory = Builder.AddCategory(editor, Resources.PageCategory);
            Builder.AddEditor(pageCategory, Resources.PageSizeProperty, i => i.PageSize);
            Builder.AddEditor(pageCategory, Resources.PagePaddingProperty, i => i.PagePadding);

            return editor;
        }
    }
}