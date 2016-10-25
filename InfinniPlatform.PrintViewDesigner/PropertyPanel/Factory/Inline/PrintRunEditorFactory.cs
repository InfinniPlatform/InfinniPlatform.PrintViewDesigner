using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Inline
{
    internal class PrintRunEditorFactory : EditorFactoryBase<PrintRun>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddInlineEditors(editor);

            var dataCategory = Builder.AddDataCategory(editor);
            Builder.AddValueFormatEditor(dataCategory, i => i.SourceFormat);
            Builder.AddLongStringEditor(dataCategory, Resources.TextProperty, i => i.Text);

            return editor;
        }
    }
}