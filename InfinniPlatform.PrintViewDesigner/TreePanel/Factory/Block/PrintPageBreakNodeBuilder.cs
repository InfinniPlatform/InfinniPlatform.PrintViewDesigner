using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Block
{
    internal class PrintPageBreakNodeBuilder : NodeBuilderBase<PrintPageBreak>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintPageBreak elementTemplate)
        {
            var pageBreakNode = parentNode.CreateChildNode(PrintElementNodeType.PrintPageBreakNode, elementTemplate);

            pageBreakNode.CanCut = NodeBuilderHelper.CanCut(pageBreakNode);
            pageBreakNode.Cut = NodeBuilderHelper.Cut(pageBreakNode);

            pageBreakNode.CanCopy = NodeBuilderHelper.CanCopy(pageBreakNode);
            pageBreakNode.Copy = NodeBuilderHelper.Copy(pageBreakNode);
        }
    }
}