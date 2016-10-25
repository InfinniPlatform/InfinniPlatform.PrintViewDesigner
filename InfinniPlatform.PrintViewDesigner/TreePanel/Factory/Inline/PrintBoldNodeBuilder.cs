using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Inline
{
    internal class PrintBoldNodeBuilder : NodeBuilderBase<PrintBold>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintBold elementTemplate)
        {
            var boldNode = parentNode.CreateChildNode(PrintElementNodeType.PrintBoldNode, elementTemplate);

            boldNode.ElementChildrenTypes = NodeBuilderHelper.InlineTypes;

            boldNode.CanCut = NodeBuilderHelper.CanCut(boldNode);
            boldNode.Cut = NodeBuilderHelper.Cut(boldNode);

            boldNode.CanCopy = NodeBuilderHelper.CanCopy(boldNode);
            boldNode.Copy = NodeBuilderHelper.Copy(boldNode);

            boldNode.CanPaste = NodeBuilderHelper.CanPaste(boldNode);
            boldNode.Paste = NodeBuilderHelper.Paste(boldNode);

            boldNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintBold, PrintInline>(boldNode);
            boldNode.InsertChild = NodeBuilderHelper.InsertChildToCollection<PrintBold, PrintInline>(factory, boldNode, p => p.Inlines);

            boldNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintBold, PrintInline>(boldNode);
            boldNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintBold, PrintInline>(boldNode, p => p.Inlines);

            boldNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintBold, PrintInline>(boldNode);
            boldNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintBold, PrintInline>(boldNode, p => p.Inlines);

            factory.CreateNodes(boldNode, elementTemplate.Inlines);
        }
    }
}