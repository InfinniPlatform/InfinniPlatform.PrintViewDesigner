using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Inline
{
    internal class PrintSpanNodeBuilder : NodeBuilderBase<PrintSpan>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintSpan elementTemplate)
        {
            var spanNode = parentNode.CreateChildNode(PrintElementNodeType.PrintSpanNode, elementTemplate);

            spanNode.ElementChildrenTypes = NodeBuilderHelper.InlineTypes;

            spanNode.CanCut = NodeBuilderHelper.CanCut(spanNode);
            spanNode.Cut = NodeBuilderHelper.Cut(spanNode);

            spanNode.CanCopy = NodeBuilderHelper.CanCopy(spanNode);
            spanNode.Copy = NodeBuilderHelper.Copy(spanNode);

            spanNode.CanPaste = NodeBuilderHelper.CanPaste(spanNode);
            spanNode.Paste = NodeBuilderHelper.Paste(spanNode);

            spanNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintSpan, PrintInline>(spanNode);
            spanNode.InsertChild = NodeBuilderHelper.InsertChildToCollection<PrintSpan, PrintInline>(factory, spanNode, p => p.Inlines);

            spanNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintSpan, PrintInline>(spanNode);
            spanNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintSpan, PrintInline>(spanNode, p => p.Inlines);

            spanNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintSpan, PrintInline>(spanNode);
            spanNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintSpan, PrintInline>(spanNode, p => p.Inlines);

            factory.CreateNodes(spanNode, elementTemplate.Inlines);
        }
    }
}