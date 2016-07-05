using System.Windows.Documents;

using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    internal sealed class FlowElementHyperlinkBuilder : IFlowElementBuilderBase<PrintElementHyperlink>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementHyperlink element, PrintElementMetadataMap elementMetadataMap)
        {
            var elementContent = new Hyperlink();

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyInlineStyles(elementContent, element);

            elementContent.NavigateUri = element.Reference;

            foreach (var inline in element.Inlines)
            {
                var inlineContent = context.Build<Inline>(inline, elementMetadataMap);

                elementContent.Inlines.Add(inlineContent);
            }

            return elementContent;
        }
    }
}
