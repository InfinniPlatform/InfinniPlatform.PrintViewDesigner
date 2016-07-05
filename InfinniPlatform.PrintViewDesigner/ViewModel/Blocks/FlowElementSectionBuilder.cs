using System.Windows.Documents;

using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Blocks
{
    internal sealed class FlowElementSectionBuilder : IFlowElementBuilderBase<PrintElementSection>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementSection element, PrintElementMetadataMap elementMetadataMap)
        {
            var elementContent = new Section();

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyBlockStyles(elementContent, element);

            foreach (var block in element.Blocks)
            {
                var newBlock = context.Build<Block>(block, elementMetadataMap);
                elementContent.Blocks.Add(newBlock);
            }

            return elementContent;
        }
    }
}
