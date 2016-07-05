using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Blocks
{
    internal sealed class PrintElementListNodeFactory : IPrintElementNodeFactory
    {
        public void Create(PrintElementNodeBuilder builder, ICollection<PrintElementNode> elements, PrintElementNode elementNode)
        {
            elementNode.CanCut = BuildHelper.CanCut(elementNode);
            elementNode.Cut = BuildHelper.Cut(elementNode);

            elementNode.CanCopy = BuildHelper.CanCopy(elementNode);
            elementNode.Copy = BuildHelper.Copy(elementNode);

            builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, "ListItems");
            builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, "ListItemTemplate");
        }
    }
}