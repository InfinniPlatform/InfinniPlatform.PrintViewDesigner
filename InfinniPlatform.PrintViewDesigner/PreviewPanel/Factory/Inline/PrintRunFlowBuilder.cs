using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Inline
{
    internal sealed class PrintRunFlowBuilder : FlowBuilderBase<PrintRun>
    {
        protected override object Build(FlowBuilderContext context, PrintRun element, PrintDocumentMap documentMap)
        {
            var flowElement = new Run { Text = element.Text };

            FlowBuilderHelper.ApplyElementStyles(flowElement, element);
            FlowBuilderHelper.ApplyInlineStyles(flowElement, element);

            return flowElement;
        }
    }
}