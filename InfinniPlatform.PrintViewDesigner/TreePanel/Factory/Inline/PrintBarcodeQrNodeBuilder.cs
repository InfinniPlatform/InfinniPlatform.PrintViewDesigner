using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Inline
{
    internal class PrintBarcodeQrNodeBuilder : NodeBuilderBase<PrintBarcodeQr>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintBarcodeQr elementTemplate)
        {
            var barcodeQrNode = parentNode.CreateChildNode(PrintElementNodeType.PrintBarcodeQrNode, elementTemplate);

            barcodeQrNode.CanCut = NodeBuilderHelper.CanCut(barcodeQrNode);
            barcodeQrNode.Cut = NodeBuilderHelper.Cut(barcodeQrNode);

            barcodeQrNode.CanCopy = NodeBuilderHelper.CanCopy(barcodeQrNode);
            barcodeQrNode.Copy = NodeBuilderHelper.Copy(barcodeQrNode);
        }
    }
}