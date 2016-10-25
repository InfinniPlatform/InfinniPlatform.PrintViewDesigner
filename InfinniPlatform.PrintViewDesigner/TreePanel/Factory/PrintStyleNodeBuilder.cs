using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory
{
    internal class PrintStyleNodeBuilder : NodeBuilderBase<PrintStyle>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintStyle elementTemplate)
        {
            var styleNode = parentNode.CreateChildNode(PrintElementNodeType.PrintStyleNode, elementTemplate);

            styleNode.CanCut = NodeBuilderHelper.CanCut(styleNode);
            styleNode.Cut = NodeBuilderHelper.Cut(styleNode);

            styleNode.CanCopy = NodeBuilderHelper.CanCopy(styleNode);
            styleNode.Copy = NodeBuilderHelper.Copy(styleNode);
        }
    }
}