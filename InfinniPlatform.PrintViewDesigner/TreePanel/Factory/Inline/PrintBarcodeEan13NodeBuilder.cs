using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Inline
{
    internal class PrintBarcodeEan13NodeBuilder : NodeBuilderBase<PrintBarcodeEan13>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintBarcodeEan13 elementTemplate)
        {
            var barcodeEan13Node = parentNode.CreateChildNode(PrintElementNodeType.PrintBarcodeEan13Node, elementTemplate);

            barcodeEan13Node.CanCut = NodeBuilderHelper.CanCut(barcodeEan13Node);
            barcodeEan13Node.Cut = NodeBuilderHelper.Cut(barcodeEan13Node);

            barcodeEan13Node.CanCopy = NodeBuilderHelper.CanCopy(barcodeEan13Node);
            barcodeEan13Node.Copy = NodeBuilderHelper.Copy(barcodeEan13Node);
        }
    }
}