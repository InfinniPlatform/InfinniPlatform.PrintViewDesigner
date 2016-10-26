using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Inline
{
    internal class PrintHyperlinkNodeBuilder : NodeBuilderBase<PrintHyperlink>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintHyperlink elementTemplate)
        {
            var hyperlinkNode = parentNode.CreateChildNode(PrintElementNodeType.PrintHyperlinkNode, elementTemplate);

            hyperlinkNode.CanCut = NodeBuilderHelper.CanCut(hyperlinkNode);
            hyperlinkNode.Cut = NodeBuilderHelper.Cut(hyperlinkNode);

            hyperlinkNode.CanCopy = NodeBuilderHelper.CanCopy(hyperlinkNode);
            hyperlinkNode.Copy = NodeBuilderHelper.Copy(hyperlinkNode);

            hyperlinkNode.CanPaste = NodeBuilderHelper.CanPaste(hyperlinkNode);
            hyperlinkNode.Paste = NodeBuilderHelper.Paste(hyperlinkNode);

            hyperlinkNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintHyperlink, PrintInline>(hyperlinkNode);
            hyperlinkNode.InsertChild = NodeBuilderHelper.InsertChildToCollection<PrintHyperlink, PrintInline>(factory, hyperlinkNode, p => p.Inlines);

            hyperlinkNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintHyperlink, PrintInline>(hyperlinkNode);
            hyperlinkNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintHyperlink, PrintInline>(hyperlinkNode, p => p.Inlines);

            hyperlinkNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintHyperlink, PrintInline>(hyperlinkNode);
            hyperlinkNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintHyperlink, PrintInline>(hyperlinkNode, p => p.Inlines);

            factory.CreateNodes(hyperlinkNode, elementTemplate.Inlines);
        }
    }
}