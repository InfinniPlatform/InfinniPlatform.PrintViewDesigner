using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory
{
    /// <summary>
    /// Базовый класс реализации <see cref="INodeBuilder"/>.
    /// </summary>
    /// <typeparam name="TElement">Тип элемента печатного представления.</typeparam>
    /// <remarks>
    /// Документ печатного представления <see cref="PrintDocument"/> представляется в виде дерева. Каждый узел этого дерева
    /// описывается с помощью экземпляра класса <see cref="PrintElementNode"/>, который связан с определенной частью документа.
    /// Узел предоставляет информацию об элементе и набор функций для его перемещения внутри документа.
    /// </remarks>
    internal abstract class NodeBuilderBase<TElement> : INodeBuilder
    {
        /// <summary>
        /// Инициализирует <see cref="PrintElementNode"/>.
        /// </summary>
        /// <param name="factory">Фабрика для создания узлов дерева.</param>
        /// <param name="parentNode">Узел дерева для отображения шаблона документа печатного представления.</param>
        /// <param name="elementTemplate">Шаблон элемента печатного представления для инициализации узла.</param>
        public void Build(NodeFactory factory, PrintElementNode parentNode, object elementTemplate)
        {
            Build(factory, parentNode, (TElement)elementTemplate);
        }

        /// <summary>
        /// Инициализирует <see cref="PrintElementNode"/>.
        /// </summary>
        /// <param name="factory">Фабрика для создания узлов дерева.</param>
        /// <param name="parentNode">Узел дерева для отображения шаблона документа печатного представления.</param>
        /// <param name="elementTemplate">Шаблон элемента печатного представления для инициализации узла.</param>
        protected abstract void Build(NodeFactory factory, PrintElementNode parentNode, TElement elementTemplate);
    }
}