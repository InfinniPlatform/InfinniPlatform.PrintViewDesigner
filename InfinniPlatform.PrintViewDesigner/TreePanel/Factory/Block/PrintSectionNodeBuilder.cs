using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Block
{
    internal class PrintSectionNodeBuilder : NodeBuilderBase<PrintSection>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintSection elementTemplate)
        {
            var sectionNode = parentNode.CreateChildNode(PrintElementNodeType.PrintSectionNode, elementTemplate);

            sectionNode.ElementChildrenTypes = NodeBuilderHelper.BlockTypes;

            sectionNode.CanCut = NodeBuilderHelper.CanCut(sectionNode);
            sectionNode.Cut = NodeBuilderHelper.Cut(sectionNode);

            sectionNode.CanCopy = NodeBuilderHelper.CanCopy(sectionNode);
            sectionNode.Copy = NodeBuilderHelper.Copy(sectionNode);

            sectionNode.CanPaste = NodeBuilderHelper.CanPaste(sectionNode);
            sectionNode.Paste = NodeBuilderHelper.Paste(sectionNode);

            sectionNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintSection, PrintBlock>(sectionNode);
            sectionNode.InsertChild = NodeBuilderHelper.InsertChildToCollection<PrintSection, PrintBlock>(factory, sectionNode, p => p.Blocks);

            sectionNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintSection, PrintBlock>(sectionNode);
            sectionNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintSection, PrintBlock>(sectionNode, p => p.Blocks);

            sectionNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintSection, PrintBlock>(sectionNode);
            sectionNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintSection, PrintBlock>(sectionNode, p => p.Blocks);

            factory.CreateNodes(sectionNode, elementTemplate.Blocks);
        }
    }
}