using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory
{
    internal class PrintDocumentNodeBuilder : NodeBuilderBase<PrintDocument>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintDocument elementTemplate)
        {
            var documentNode = parentNode.CreateChildNode(PrintElementNodeType.PrintDocumentNode, elementTemplate);

            BuildStyles(factory, documentNode, elementTemplate);
            BuildBlocks(factory, documentNode, elementTemplate);
        }


        private static void BuildStyles(NodeFactory factory, PrintElementNode documentNode, PrintDocument document)
        {
            var stylesNode = documentNode.CreateChildNode(PrintElementNodeType.PrintDocumentStylesNode, document);

            stylesNode.CanPaste = NodeBuilderHelper.CanPaste(stylesNode);
            stylesNode.Paste = NodeBuilderHelper.Paste(stylesNode);

            stylesNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintDocument, PrintStyle>(stylesNode);
            stylesNode.InsertChild = NodeBuilderHelper.InsertChildToCollection<PrintDocument, PrintStyle>(factory, stylesNode, p => p.Styles);

            stylesNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintDocument, PrintStyle>(stylesNode);
            stylesNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintDocument, PrintStyle>(stylesNode, p => p.Styles);

            stylesNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintDocument, PrintStyle>(stylesNode);
            stylesNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintDocument, PrintStyle>(stylesNode, p => p.Styles);

            factory.CreateNodes(stylesNode, document.Styles);
        }


        private static void BuildBlocks(NodeFactory factory, PrintElementNode documentNode, PrintDocument document)
        {
            var blocksNode = documentNode.CreateChildNode(PrintElementNodeType.PrintDocumentBlocksNode, document);

            blocksNode.CanPaste = NodeBuilderHelper.CanPaste(blocksNode);
            blocksNode.Paste = NodeBuilderHelper.Paste(blocksNode);

            blocksNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintDocument, PrintBlock>(blocksNode);
            blocksNode.InsertChild = NodeBuilderHelper.InsertChildToCollection<PrintDocument, PrintBlock>(factory, blocksNode, p => p.Blocks);

            blocksNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintDocument, PrintBlock>(blocksNode);
            blocksNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintDocument, PrintBlock>(blocksNode, p => p.Blocks);

            blocksNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintDocument, PrintBlock>(blocksNode);
            blocksNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintDocument, PrintBlock>(blocksNode, p => p.Blocks);

            factory.CreateNodes(blocksNode, document.Blocks);
        }
    }
}