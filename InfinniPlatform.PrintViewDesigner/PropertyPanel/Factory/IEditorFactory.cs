using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal interface IEditorFactory
    {
        PropertyEditorBase Create(string caption);
    }
}