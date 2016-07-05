﻿using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Blocks
{
    internal sealed class PrintElementLineNodeFactory : IPrintElementNodeFactory
    {
        public void Create(PrintElementNodeBuilder builder, ICollection<PrintElementNode> elements, PrintElementNode elementNode)
        {
            elementNode.CanCut = BuildHelper.CanCut(elementNode);
            elementNode.Cut = BuildHelper.Cut(elementNode);

            elementNode.CanCopy = BuildHelper.CanCopy(elementNode);
            elementNode.Copy = BuildHelper.Copy(elementNode);
        }
    }
}