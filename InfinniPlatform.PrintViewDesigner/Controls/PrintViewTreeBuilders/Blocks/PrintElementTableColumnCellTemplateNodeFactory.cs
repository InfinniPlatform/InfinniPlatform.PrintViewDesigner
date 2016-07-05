using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Blocks
{
    internal sealed class PrintElementTableColumnCellTemplateNodeFactory : IPrintElementNodeFactory
    {
        private static readonly string[] TableCells = {"TableCell"};

        public void Create(PrintElementNodeBuilder builder, ICollection<PrintElementNode> elements, PrintElementNode elementNode)
        {
            elementNode.ElementChildrenTypes = TableCells;
            elementNode.CanInsertChild = BuildHelper.CanInsertChild(elementNode);
            elementNode.InsertChild = BuildHelper.InsertChildToContainer(builder, elements, elementNode, "CellTemplate",
                false);

            elementNode.CanDeleteChild = BuildHelper.CanDeleteChild(elementNode);
            elementNode.DeleteChild = BuildHelper.DeleteChildFromContainer(elements, elementNode, "CellTemplate");

            elementNode.CanPaste = BuildHelper.CanPaste(elementNode);
            elementNode.Paste = BuildHelper.Paste(elementNode);

            builder.BuildElement(elements, elementNode, elementNode.ElementMetadata.CellTemplate, "TableCell");
        }
    }
}