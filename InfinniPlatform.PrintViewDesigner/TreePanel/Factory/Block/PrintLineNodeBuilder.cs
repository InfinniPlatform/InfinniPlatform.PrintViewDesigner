using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Block
{
    internal class PrintLineNodeBuilder : NodeBuilderBase<PrintLine>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintLine elementTemplate)
        {
            var lineNode = parentNode.CreateChildNode(PrintElementNodeType.PrintLineNode, elementTemplate);

            lineNode.CanCut = NodeBuilderHelper.CanCut(lineNode);
            lineNode.Cut = NodeBuilderHelper.Cut(lineNode);

            lineNode.CanCopy = NodeBuilderHelper.CanCopy(lineNode);
            lineNode.Copy = NodeBuilderHelper.Copy(lineNode);
        }
    }
}