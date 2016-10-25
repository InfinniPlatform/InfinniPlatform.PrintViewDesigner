using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal abstract class EditorFactoryBase<T> : IEditorFactory
    {
        public EditorBuilder<T> Builder { get; set; }

        public abstract PropertyEditorBase Create(string caption);
    }
}