using System.Windows.Documents;

using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    internal sealed class FlowElementBoldBuilder : IFlowElementBuilderBase<PrintElementBold>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementBold element, PrintElementMetadataMap elementMetadataMap)
        {
            var elementContent = new Bold();

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyInlineStyles(elementContent, element);

            foreach (var inline in element.Inlines)
            {
                var inlineContent = context.Build<Inline>(inline, elementMetadataMap);

                elementContent.Inlines.Add(inlineContent);
            }

            return elementContent;
        }
    }
}
