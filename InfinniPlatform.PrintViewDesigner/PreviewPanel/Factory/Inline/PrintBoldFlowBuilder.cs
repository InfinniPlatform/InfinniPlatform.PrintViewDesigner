using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Inline;

using InlineElement = System.Windows.Documents.Inline;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Inline
{
    internal sealed class PrintBoldFlowBuilder : FlowBuilderBase<PrintBold>
    {
        protected override object Build(FlowBuilderContext context, PrintBold element, PrintDocumentMap documentMap)
        {
            var flowElement = new Bold();

            FlowBuilderHelper.ApplyElementStyles(flowElement, element);
            FlowBuilderHelper.ApplyInlineStyles(flowElement, element);

            if (element.Inlines != null)
            {
                foreach (var inline in element.Inlines)
                {
                    var flowInline = context.Build<InlineElement>(inline, documentMap);

                    if (flowInline != null)
                    {
                        flowElement.Inlines.Add(flowInline);
                    }
                }
            }

            return flowElement;
        }
    }
}