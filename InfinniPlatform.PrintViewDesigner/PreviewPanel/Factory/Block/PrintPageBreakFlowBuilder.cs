using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Block
{
    internal sealed class PrintPageBreakFlowBuilder : FlowBuilderBase<PrintPageBreak>
    {
        protected override object Build(FlowBuilderContext context, PrintPageBreak element, PrintDocumentMap documentMap)
        {
            var flowElement = new Paragraph { FontSize = FlowBuilderHelper.FontSizeMin, BreakPageBefore = true };

            FlowBuilderHelper.ApplyElementStyles(flowElement, element);
            FlowBuilderHelper.ApplyBlockStyles(flowElement, element);

            return flowElement;
        }
    }
}