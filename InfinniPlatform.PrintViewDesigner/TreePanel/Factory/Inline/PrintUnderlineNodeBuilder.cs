using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Inline
{
    internal class PrintUnderlineNodeBuilder : NodeBuilderBase<PrintUnderline>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintUnderline elementTemplate)
        {
            var underlineNode = parentNode.CreateChildNode(PrintElementNodeType.PrintUnderlineNode, elementTemplate);

            underlineNode.CanCut = NodeBuilderHelper.CanCut(underlineNode);
            underlineNode.Cut = NodeBuilderHelper.Cut(underlineNode);

            underlineNode.CanCopy = NodeBuilderHelper.CanCopy(underlineNode);
            underlineNode.Copy = NodeBuilderHelper.Copy(underlineNode);

            underlineNode.CanPaste = NodeBuilderHelper.CanPaste(underlineNode);
            underlineNode.Paste = NodeBuilderHelper.Paste(underlineNode);

            underlineNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintUnderline, PrintInline>(underlineNode);
            underlineNode.InsertChild = NodeBuilderHelper.InsertChildToCollection<PrintUnderline, PrintInline>(factory, underlineNode, p => p.Inlines);

            underlineNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintUnderline, PrintInline>(underlineNode);
            underlineNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintUnderline, PrintInline>(underlineNode, p => p.Inlines);

            underlineNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintUnderline, PrintInline>(underlineNode);
            underlineNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintUnderline, PrintInline>(underlineNode, p => p.Inlines);

            factory.CreateNodes(underlineNode, elementTemplate.Inlines);
        }
    }
}