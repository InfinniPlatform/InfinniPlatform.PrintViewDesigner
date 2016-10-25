using System.Windows;
using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Block;

using BlockElemet = System.Windows.Documents.Block;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Block
{
    internal sealed class PrintListFlowBuilder : FlowBuilderBase<PrintList>
    {
        private const int ListMarkerOffset = 7 + 5;


        protected override object Build(FlowBuilderContext context, PrintList element, PrintDocumentMap documentMap)
        {
            var flowElement = new List { MarkerStyle = TextMarkerStyle.None, MarkerOffset = ListMarkerOffset };

            FlowBuilderHelper.ApplyElementStyles(flowElement, element);
            FlowBuilderHelper.ApplyBlockStyles(flowElement, element);

            if (element.StartIndex != null)
            {
                flowElement.StartIndex = element.StartIndex.Value;
            }

            if (element.MarkerStyle != null)
            {
                flowElement.MarkerStyle = FlowBuilderHelper.GetEnumValue<TextMarkerStyle>(element.MarkerStyle);
            }

            if (element.MarkerOffsetSize != null)
            {
                flowElement.MarkerOffset += FlowBuilderHelper.GetSizeInPixels(element.MarkerOffsetSize, element.MarkerOffsetSizeUnit);
            }

            if (element.Items != null)
            {
                foreach (var item in element.Items)
                {
                    var flowItem = context.Build<BlockElemet>(item, documentMap);

                    if (flowItem != null)
                    {
                        var listItem = new ListItem();
                        listItem.Blocks.Add(flowItem);
                        flowElement.ListItems.Add(listItem);
                    }
                }
            }

            return flowElement;
        }
    }
}