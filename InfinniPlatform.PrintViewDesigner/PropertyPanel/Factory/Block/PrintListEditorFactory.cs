using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Block
{
    internal class PrintListEditorFactory : EditorFactoryBase<PrintList>
    {
        public override PropertyEditorBase Create(string caption)
        {
            var editor = new PropertyEditor { Caption = caption };

            Builder.AddBlockEditors(editor);

            var markerOffsetSizeEditor = new MarkerOffsetSizeEditor { Caption = Resources.ListMarkerOffset };
            Builder.AddEditor(markerOffsetSizeEditor, markerOffsetSizeEditor.SizeEditor, i => i.MarkerOffsetSize);
            Builder.AddEditor(markerOffsetSizeEditor, markerOffsetSizeEditor.SizeUnitEditor, i => i.MarkerOffsetSizeUnit);

            var listCategory = Builder.AddCategory(editor, Resources.ListCategory);
            Builder.AddIntegerEditor(listCategory, Resources.ListStartIndexProperty, i => i.StartIndex, 1);
            Builder.AddEditor(listCategory, markerOffsetSizeEditor);
            Builder.AddEnumEditor(listCategory, Resources.ListMarkerStyleProperty, i => i.MarkerStyle);

            return editor;
        }
    }
}