using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Block
{
    internal class PrintTableCellNodeBuilder : NodeBuilderBase<PrintTableCell>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintTableCell elementTemplate)
        {
            var cellNode = parentNode.CreateChildNode(PrintElementNodeType.PrintTableCellNode, elementTemplate);

            cellNode.CanCut = NodeBuilderHelper.CanCut(cellNode);
            cellNode.Cut = NodeBuilderHelper.Cut(cellNode);

            cellNode.CanCopy = NodeBuilderHelper.CanCopy(cellNode);
            cellNode.Copy = NodeBuilderHelper.Copy(cellNode);

            cellNode.CanPaste = NodeBuilderHelper.CanPaste(cellNode);
            cellNode.Paste = NodeBuilderHelper.Paste(cellNode);

            cellNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintTableCell, PrintBlock>(cellNode);
            cellNode.InsertChild = NodeBuilderHelper.InsertChildToContainer<PrintTableCell, PrintBlock>(factory, cellNode, (p, c) => p.Block = c);

            cellNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintTableCell, PrintBlock>(cellNode);
            cellNode.DeleteChild = NodeBuilderHelper.DeleteChildFromContainer<PrintTableCell, PrintBlock>(cellNode, (p, c) => p.Block = null);

            factory.CreateNode(cellNode, elementTemplate.Block);
        }
    }
}