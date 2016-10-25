using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Block
{
    internal class PrintTableColumnNodeBuilder : NodeBuilderBase<PrintTableColumn>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintTableColumn elementTemplate)
        {
            var columnNode = parentNode.CreateChildNode(PrintElementNodeType.PrintTableColumnNode, elementTemplate);

            columnNode.CanCut = NodeBuilderHelper.CanCut(columnNode);
            columnNode.Cut = NodeBuilderHelper.Cut(columnNode);

            columnNode.CanCopy = NodeBuilderHelper.CanCopy(columnNode);
            columnNode.Copy = NodeBuilderHelper.Copy(columnNode);

            BuildTableColumnHeader(factory, columnNode, elementTemplate);
            BuildTableColumnCellTemplate(factory, columnNode, elementTemplate);
        }


        private static void BuildTableColumnHeader(NodeFactory factory, PrintElementNode columnNode, PrintTableColumn column)
        {
            var headerNode = columnNode.CreateChildNode(PrintElementNodeType.PrintTableColumnHeaderNode, column);

            headerNode.ElementChildrenTypes = new[] { PrintElementNodeType.PrintTableCellNode };

            headerNode.CanPaste = NodeBuilderHelper.CanPaste(headerNode);
            headerNode.Paste = NodeBuilderHelper.Paste(headerNode);

            headerNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintTableColumn, PrintTableCell>(headerNode);
            headerNode.InsertChild = NodeBuilderHelper.InsertChildToContainer<PrintTableColumn, PrintTableCell>(factory, headerNode, (p, c) => p.Header = c);

            headerNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintTableColumn, PrintTableCell>(headerNode);
            headerNode.DeleteChild = NodeBuilderHelper.DeleteChildFromContainer<PrintTableColumn, PrintTableCell>(headerNode, (p, c) => p.Header = null);

            factory.CreateNode(headerNode, column.Header);
        }

        private static void BuildTableColumnCellTemplate(NodeFactory factory, PrintElementNode columnNode, PrintTableColumn column)
        {
            var cellTemplateNode = columnNode.CreateChildNode(PrintElementNodeType.PrintTableColumnCellTemplateNode, column);

            cellTemplateNode.ElementChildrenTypes = new[] { PrintElementNodeType.PrintTableCellNode };

            cellTemplateNode.CanPaste = NodeBuilderHelper.CanPaste(cellTemplateNode);
            cellTemplateNode.Paste = NodeBuilderHelper.Paste(cellTemplateNode);

            cellTemplateNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintTableColumn, PrintTableCell>(cellTemplateNode);
            cellTemplateNode.InsertChild = NodeBuilderHelper.InsertChildToContainer<PrintTableColumn, PrintTableCell>(factory, cellTemplateNode, (p, c) => p.CellTemplate = c);

            cellTemplateNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintTableColumn, PrintTableCell>(cellTemplateNode);
            cellTemplateNode.DeleteChild = NodeBuilderHelper.DeleteChildFromContainer<PrintTableColumn, PrintTableCell>(cellTemplateNode, (p, c) => p.CellTemplate = null);

            factory.CreateNode(cellTemplateNode, column.CellTemplate);
        }
    }
}