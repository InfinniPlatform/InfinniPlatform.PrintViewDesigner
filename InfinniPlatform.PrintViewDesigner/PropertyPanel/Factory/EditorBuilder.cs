using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal class EditorBuilder<T>
    {
        public EditorBuilder(Func<PrintDocument> template, EditorBuilderContext context)
        {
            Template = template;
            Context = context;
        }


        public Func<PrintDocument> Template { get; }

        public EditorBuilderContext Context { get; }

        public ICollection<PropertyEditorBase> Editors { get; set; }


        public void AddEditor<TProperty>(PropertyEditorBase parent, string caption, Expression<Func<T, TProperty>> property) where TProperty : new()
        {
            var editor = Context.Create<TProperty>(Editors, caption);

            AddEditor(parent, editor, property);
        }

        public void AddEditor<TProperty>(PropertyEditorBase parent, PropertyEditorBase editor, Expression<Func<T, TProperty>> property)
        {
            editor.Property = ((MemberExpression)property.Body).Member.Name;

            AddEditor(parent, editor);
        }

        public void AddEditor(PropertyEditorBase parent, PropertyEditorBase editor)
        {
            parent.AddEditor(Editors, editor);
        }

        public PropertyEditorBase AddCategory(PropertyEditorBase parent, string caption)
        {
            return parent.AddCategory(Editors, caption);
        }
    }
}