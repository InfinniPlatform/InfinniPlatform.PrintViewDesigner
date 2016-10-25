using System;
using System.Collections.Generic;

using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory
{
    /// <summary>
    /// Фабрика для создания <see cref="PrintElementNode"/>.
    /// </summary>
    /// <remarks>
    /// Документ печатного представления <see cref="PrintDocument"/> представляется в виде дерева. Каждый узел этого дерева
    /// описывается с помощью экземпляра класса <see cref="PrintElementNode"/>, который связан с определенной частью документа.
    /// Узел предоставляет информацию об элементе и набор функций для его перемещения внутри документа.
    /// </remarks>
    internal class NodeFactory
    {
        private readonly Dictionary<Type, INodeBuilder> _builders
            = new Dictionary<Type, INodeBuilder>();


        public void Register<TElement>(NodeBuilderBase<TElement> builder)
        {
            _builders.Add(typeof(TElement), builder);
        }


        public void CreateNode(PrintElementNode parentNode, object elementTemplate)
        {
            if (elementTemplate != null)
            {
                INodeBuilder builder;

                if (_builders.TryGetValue(elementTemplate.GetType(), out builder))
                {
                    builder.Build(this, parentNode, elementTemplate);
                }
            }
        }


        public void CreateNodes<TElement>(PrintElementNode parentNode, IEnumerable<TElement> elementTemplates)
        {
            if (elementTemplates != null)
            {
                foreach (var elementTemplate in elementTemplates)
                {
                    CreateNode(parentNode, elementTemplate);
                }
            }
        }
    }
}