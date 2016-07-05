using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Views
{
    internal sealed class PrintViewNodeFactory : IPrintElementNodeFactory
    {
        public void Create(PrintElementNodeBuilder builder, ICollection<PrintElementNode> elements, PrintElementNode elementNode)
        {
            builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, "PrintViewStyles");
            builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, "PrintViewBlocks");
        }
    }
}