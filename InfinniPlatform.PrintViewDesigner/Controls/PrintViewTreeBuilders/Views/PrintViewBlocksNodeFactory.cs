using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Views
{
    internal sealed class PrintViewBlocksNodeFactory : IPrintElementNodeFactory
    {
        public void Create(PrintElementNodeBuilder builder, ICollection<PrintElementNode> elements, PrintElementNode elementNode)
        {
            elementNode.ElementChildrenTypes = BuildHelper.BlockTypes;
            elementNode.CanInsertChild = BuildHelper.CanInsertChild(elementNode);
            elementNode.InsertChild = BuildHelper.InsertChildToCollection(builder, elements, elementNode, "Blocks");

            elementNode.CanDeleteChild = BuildHelper.CanDeleteChild(elementNode);
            elementNode.DeleteChild = BuildHelper.DeleteChildFromCollection(elements, elementNode, "Blocks");

            elementNode.CanMoveChild = BuildHelper.CanMoveChild(elementNode);
            elementNode.MoveChild = BuildHelper.MoveChildInCollection(elementNode, "Blocks");

            elementNode.CanPaste = BuildHelper.CanPaste(elementNode);
            elementNode.Paste = BuildHelper.Paste(elementNode);

            builder.BuildElements(elements, elementNode, elementNode.ElementMetadata.Blocks);
        }
    }
}