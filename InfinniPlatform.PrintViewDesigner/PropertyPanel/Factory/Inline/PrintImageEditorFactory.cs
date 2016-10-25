using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Inline
{
    internal class PrintImageEditorFactory : EditorFactoryBase<PrintImage>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddCommonEditors(editor);

            var dataCategory = Builder.AddDataCategory(editor);
            var imageEditor = new ImageEditor { Caption = Resources.ImageDataProperty };
            Builder.AddEditor(dataCategory, imageEditor, i => i.Data);

            var imageCategory = Builder.AddCategory(editor, Resources.ImageCategory);
            Builder.AddEditor(imageCategory, Resources.ImageSizeProperty, i => i.Size);
            Builder.AddImageRotation(imageCategory, i => i.Rotation);
            Builder.AddEnumEditor(imageCategory, Resources.ImageStretchProperty, i => i.Stretch);

            return editor;
        }
    }
}