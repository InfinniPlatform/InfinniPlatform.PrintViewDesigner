using System;
using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Inline;

using InlineElement = System.Windows.Documents.Inline;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Inline
{
    internal sealed class PrintHyperlinkFlowBuilder : FlowBuilderBase<PrintHyperlink>
    {
        protected override object Build(FlowBuilderContext context, PrintHyperlink element, PrintDocumentMap documentMap)
        {
            var flowElement = new Hyperlink();

            FlowBuilderHelper.ApplyElementStyles(flowElement, element);
            FlowBuilderHelper.ApplyInlineStyles(flowElement, element);

            flowElement.NavigateUri = TryCreateUri(element.Reference);

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


        private static Uri TryCreateUri(string reference)
        {
            Uri referenceUri;

            Uri.TryCreate(reference, UriKind.RelativeOrAbsolute, out referenceUri);

            return referenceUri;
        }
    }
}