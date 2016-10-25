using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Block;

using InlineElement = System.Windows.Documents.Inline;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Block
{
    internal sealed class PrintParagraphFlowBuilder : FlowBuilderBase<PrintParagraph>
    {
        protected override object Build(FlowBuilderContext context, PrintParagraph element, PrintDocumentMap documentMap)
        {
            var flowElement = new Paragraph();

            FlowBuilderHelper.ApplyElementStyles(flowElement, element);
            FlowBuilderHelper.ApplyBlockStyles(flowElement, element);

            if (element.IndentSize != null)
            {
                flowElement.TextIndent = FlowBuilderHelper.GetSizeInPixels(element.IndentSize, element.IndentSizeUnit);
            }

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