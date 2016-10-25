using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Inline
{
    internal class PrintLineBreakNodeBuilder : NodeBuilderBase<PrintLineBreak>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintLineBreak elementTemplate)
        {
            var lineBreakNode = parentNode.CreateChildNode(PrintElementNodeType.PrintLineBreakNode, elementTemplate);

            lineBreakNode.CanCut = NodeBuilderHelper.CanCut(lineBreakNode);
            lineBreakNode.Cut = NodeBuilderHelper.Cut(lineBreakNode);

            lineBreakNode.CanCopy = NodeBuilderHelper.CanCopy(lineBreakNode);
            lineBreakNode.Copy = NodeBuilderHelper.Copy(lineBreakNode);
        }
    }
}