using System;
using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal class EditorBuilderContext
    {
        private readonly Dictionary<Type, IEditorFactory> _factories
            = new Dictionary<Type, IEditorFactory>();


        public void Register<T>(EditorFactoryBase<T> factory)
        {
            _factories.Add(typeof(T), factory);
        }


        public PropertyEditorBase Create<T>(ICollection<PropertyEditorBase> editors, string caption = null) where T : new()
        {
            var factory = (EditorFactoryBase<T>)_factories[typeof(T)];

            factory.Builder.Editors = editors;

            var editor = factory.Create(caption);

            editor.CreateNewEditValue = () => new T();

            return editor;
        }
    }
}