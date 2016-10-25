using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Block
{
    internal class PrintParagraphNodeBuilder : NodeBuilderBase<PrintParagraph>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintParagraph elementTemplate)
        {
            var paragraphNode = parentNode.CreateChildNode(PrintElementNodeType.PrintParagraphNode, elementTemplate);

            paragraphNode.ElementChildrenTypes = NodeBuilderHelper.InlineTypes;

            paragraphNode.CanCut = NodeBuilderHelper.CanCut(paragraphNode);
            paragraphNode.Cut = NodeBuilderHelper.Cut(paragraphNode);

            paragraphNode.CanCopy = NodeBuilderHelper.CanCopy(paragraphNode);
            paragraphNode.Copy = NodeBuilderHelper.Copy(paragraphNode);

            paragraphNode.CanPaste = NodeBuilderHelper.CanPaste(paragraphNode);
            paragraphNode.Paste = NodeBuilderHelper.Paste(paragraphNode);

            paragraphNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintParagraph, PrintInline>(paragraphNode);
            paragraphNode.InsertChild = NodeBuilderHelper.InsertChildToCollection<PrintParagraph, PrintInline>(factory, paragraphNode, p => p.Inlines);

            paragraphNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintParagraph, PrintInline>(paragraphNode);
            paragraphNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintParagraph, PrintInline>(paragraphNode, p => p.Inlines);

            paragraphNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintParagraph, PrintInline>(paragraphNode);
            paragraphNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintParagraph, PrintInline>(paragraphNode, p => p.Inlines);

            factory.CreateNodes(paragraphNode, elementTemplate.Inlines);
        }
    }
}