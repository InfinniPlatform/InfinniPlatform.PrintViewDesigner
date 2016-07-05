using System.Windows.Documents;

using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    internal sealed class FlowElementLineBreakBuilder : IFlowElementBuilderBase<PrintElementLineBreak>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementLineBreak element, PrintElementMetadataMap elementMetadataMap)
        {
            var elementContent = new LineBreak();

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyInlineStyles(elementContent, element);

            return elementContent;
        }
    }
}
