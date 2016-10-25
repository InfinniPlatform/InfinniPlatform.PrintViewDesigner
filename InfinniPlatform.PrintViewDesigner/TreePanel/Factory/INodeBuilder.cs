using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory
{
    /// <summary>
    /// Предоставляет метод инициализации <see cref="PrintElementNode"/>.
    /// </summary>
    /// <remarks>
    /// Документ печатного представления <see cref="PrintDocument"/> представляется в виде дерева. Каждый узел этого дерева
    /// описывается с помощью экземпляра класса <see cref="PrintElementNode"/>, который связан с определенной частью документа.
    /// Узел предоставляет информацию об элементе и набор функций для его перемещения внутри документа.
    /// </remarks>
    internal interface INodeBuilder
    {
        /// <summary>
        /// Инициализирует <see cref="PrintElementNode"/>.
        /// </summary>
        /// <param name="factory">Фабрика для создания узлов дерева.</param>
        /// <param name="parentNode">Узел дерева для отображения шаблона документа печатного представления.</param>
        /// <param name="elementTemplate">Шаблон элемента печатного представления для инициализации узла.</param>
        void Build(NodeFactory factory, PrintElementNode parentNode, object elementTemplate);
    }
}