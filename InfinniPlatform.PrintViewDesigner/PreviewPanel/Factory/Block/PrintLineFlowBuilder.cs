using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Block
{
    internal sealed class PrintLineFlowBuilder : FlowBuilderBase<PrintLine>
    {
        protected override object Build(FlowBuilderContext context, PrintLine element, PrintDocumentMap documentMap)
        {
            var flowElement = new Paragraph { FontSize = FlowBuilderHelper.FontSizeMin };

            FlowBuilderHelper.ApplyElementStyles(flowElement, element);
            FlowBuilderHelper.ApplyBlockStyles(flowElement, element);

            return flowElement;
        }
    }
}