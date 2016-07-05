using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Blocks
{
    internal sealed class PrintElementSectionNodeFactory : IPrintElementNodeFactory
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

            elementNode.CanCut = BuildHelper.CanCut(elementNode);
            elementNode.Cut = BuildHelper.Cut(elementNode);

            elementNode.CanCopy = BuildHelper.CanCopy(elementNode);
            elementNode.Copy = BuildHelper.Copy(elementNode);

            elementNode.CanPaste = BuildHelper.CanPaste(elementNode);
            elementNode.Paste = BuildHelper.Paste(elementNode);

            builder.BuildElements(elements, elementNode, elementNode.ElementMetadata.Blocks);
        }
    }
}