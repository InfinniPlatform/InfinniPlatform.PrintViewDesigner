using System.Collections.Generic;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.TreePanel.Factory;
using InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Block;
using InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Panel
{
    /// <summary>
    /// Фабрика для построения дерева шаблона печатного представления <see cref="PrintDocument"/>.
    /// </summary>
    /// <remarks>
    /// Документ печатного представления <see cref="PrintDocument"/> представляется в виде дерева. Каждый узел этого дерева
    /// описывается с помощью экземпляра класса <see cref="PrintElementNode"/>, который связан с определенной частью документа.
    /// Узел предоставляет информацию об элементе и набор функций для его перемещения внутри документа.
    /// </remarks>
    public class PrintViewTreeFactory
    {
        private static readonly NodeFactory Factory;


        static PrintViewTreeFactory()
        {
            Factory = new NodeFactory();

            Factory.Register(new PrintDocumentNodeBuilder());
            Factory.Register(new PrintStyleNodeBuilder());

            // Block
            Factory.Register(new PrintLineNodeBuilder());
            Factory.Register(new PrintListNodeBuilder());
            Factory.Register(new PrintPageBreakNodeBuilder());
            Factory.Register(new PrintParagraphNodeBuilder());
            Factory.Register(new PrintSectionNodeBuilder());
            Factory.Register(new PrintTableNodeBuilder());
            Factory.Register(new PrintTableColumnNodeBuilder());
            Factory.Register(new PrintTableRowNodeBuilder());
            Factory.Register(new PrintTableCellNodeBuilder());

            // Inline
            Factory.Register(new PrintBarcodeEan13NodeBuilder());
            Factory.Register(new PrintBarcodeQrNodeBuilder());
            Factory.Register(new PrintBoldNodeBuilder());
            Factory.Register(new PrintHyperlinkNodeBuilder());
            Factory.Register(new PrintImageNodeBuilder());
            Factory.Register(new PrintItalicNodeBuilder());
            Factory.Register(new PrintLineBreakNodeBuilder());
            Factory.Register(new PrintRunNodeBuilder());
            Factory.Register(new PrintSpanNodeBuilder());
            Factory.Register(new PrintUnderlineNodeBuilder());
        }


        public void CreateTree(ICollection<PrintElementNode> tree, PrintDocument document)
        {
            var rootNode = new PrintElementNode(tree, parentNode: null, elementType: default(PrintElementNodeType), elementTemplate: null);

            Factory.CreateNode(rootNode, document);
        }
    }
}