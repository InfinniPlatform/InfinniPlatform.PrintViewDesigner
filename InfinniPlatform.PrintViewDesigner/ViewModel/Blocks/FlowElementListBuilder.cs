using System.Windows;
using System.Windows.Documents;

using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Blocks
{
    internal sealed class FlowElementListBuilder : IFlowElementBuilderBase<PrintElementList>
    {
        private const int DefaultMarkerOffsetSize = 7 + 5;

        public override object Build(FlowElementBuilderContext context, PrintElementList element, PrintElementMetadataMap elementMetadataMap)
        {
            var elementContent = new List
            {
                MarkerStyle = TextMarkerStyle.None,
                MarkerOffset = element.MarkerOffsetSize + DefaultMarkerOffsetSize
            };

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyBlockStyles(elementContent, element);

            if (element.StartIndex != null)
            {
                elementContent.StartIndex = element.StartIndex.Value;
            }

            if (element.MarkerStyle != null)
            {
                elementContent.MarkerStyle = FlowElementBuilderHelper.GetMarkerStyle(element.MarkerStyle.Value);
            }

            foreach (var item in element.Items)
            {
                var itemContent = context.Build<Block>(item, elementMetadataMap);

                if (itemContent != null)
                {
                    var listItem = new ListItem();
                    listItem.Blocks.Add(itemContent);
                    elementContent.ListItems.Add(listItem);
                }
            }

            return elementContent;
        }
    }
}
