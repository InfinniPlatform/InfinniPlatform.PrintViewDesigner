using System.Windows.Documents;

using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    internal sealed class FlowElementUnderlineBuilder : IFlowElementBuilderBase<PrintElementUnderline>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementUnderline element, PrintElementMetadataMap elementMetadataMap)
        {
            var elementContent = new Underline();

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
