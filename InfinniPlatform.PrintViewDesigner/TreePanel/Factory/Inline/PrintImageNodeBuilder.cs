using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Inline
{
    internal class PrintImageNodeBuilder : NodeBuilderBase<PrintImage>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintImage elementTemplate)
        {
            var imageNode = parentNode.CreateChildNode(PrintElementNodeType.PrintImageNode, elementTemplate);

            imageNode.CanCut = NodeBuilderHelper.CanCut(imageNode);
            imageNode.Cut = NodeBuilderHelper.Cut(imageNode);

            imageNode.CanCopy = NodeBuilderHelper.CanCopy(imageNode);
            imageNode.Copy = NodeBuilderHelper.Copy(imageNode);
        }
    }
}