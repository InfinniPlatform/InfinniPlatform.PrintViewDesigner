using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Inline
{
    internal class PrintItalicNodeBuilder : NodeBuilderBase<PrintItalic>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintItalic elementTemplate)
        {
            var italicNode = parentNode.CreateChildNode(PrintElementNodeType.PrintItalicNode, elementTemplate);

            italicNode.ElementChildrenTypes = NodeBuilderHelper.InlineTypes;

            italicNode.CanCut = NodeBuilderHelper.CanCut(italicNode);
            italicNode.Cut = NodeBuilderHelper.Cut(italicNode);

            italicNode.CanCopy = NodeBuilderHelper.CanCopy(italicNode);
            italicNode.Copy = NodeBuilderHelper.Copy(italicNode);

            italicNode.CanPaste = NodeBuilderHelper.CanPaste(italicNode);
            italicNode.Paste = NodeBuilderHelper.Paste(italicNode);

            italicNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintItalic, PrintInline>(italicNode);
            italicNode.InsertChild = NodeBuilderHelper.InsertChildToCollection<PrintItalic, PrintInline>(factory, italicNode, p => p.Inlines);

            italicNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintItalic, PrintInline>(italicNode);
            italicNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintItalic, PrintInline>(italicNode, p => p.Inlines);

            italicNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintItalic, PrintInline>(italicNode);
            italicNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintItalic, PrintInline>(italicNode, p => p.Inlines);

            factory.CreateNodes(italicNode, elementTemplate.Inlines);
        }
    }
}