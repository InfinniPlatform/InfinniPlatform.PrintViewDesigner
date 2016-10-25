using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Inline
{
    internal sealed class PrintLineBreakFlowBuilder : FlowBuilderBase<PrintLineBreak>
    {
        protected override object Build(FlowBuilderContext context, PrintLineBreak element, PrintDocumentMap documentMap)
        {
            var flowElement = new LineBreak();

            FlowBuilderHelper.ApplyElementStyles(flowElement, element);
            FlowBuilderHelper.ApplyInlineStyles(flowElement, element);

            return flowElement;
        }
    }
}