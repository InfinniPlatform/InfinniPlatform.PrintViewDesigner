using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Inline
{
    internal class PrintRunNodeBuilder : NodeBuilderBase<PrintRun>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintRun elementTemplate)
        {
            var runNode = parentNode.CreateChildNode(PrintElementNodeType.PrintRunNode, elementTemplate);

            runNode.CanCut = NodeBuilderHelper.CanCut(runNode);
            runNode.Cut = NodeBuilderHelper.Cut(runNode);

            runNode.CanCopy = NodeBuilderHelper.CanCopy(runNode);
            runNode.Copy = NodeBuilderHelper.Copy(runNode);
        }
    }
}