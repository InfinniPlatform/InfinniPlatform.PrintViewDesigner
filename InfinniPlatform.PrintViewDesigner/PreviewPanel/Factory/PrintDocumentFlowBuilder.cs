using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model;

using BlockElement = System.Windows.Documents.Block;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory
{
    internal sealed class PrintDocumentFlowBuilder : FlowBuilderBase<PrintDocument>
    {
        private const double RichTextBoxDefaultPadding = 10;


        protected override object Build(FlowBuilderContext context, PrintDocument element, PrintDocumentMap documentMap)
        {
            var flowElement = new FlowDocument();

            ApplyDocumentStyles(flowElement, element);

            var pageSize = element.PageSize;

            if (pageSize != null)
            {
                var paddingLeft = 0d;
                var paddingRigt = 0d;
                var pagePaddig = element.PagePadding;

                if (pagePaddig != null)
                {
                    paddingLeft = FlowBuilderHelper.GetSizeInPixels(pagePaddig.Left, pagePaddig.SizeUnit);
                    paddingRigt = FlowBuilderHelper.GetSizeInPixels(pagePaddig.Right, pagePaddig.SizeUnit);
                }

                var pageWidth = FlowBuilderHelper.GetSizeInPixels(pageSize.Width, pageSize.SizeUnit);
                var pageHeight = FlowBuilderHelper.GetSizeInPixels(pageSize.Height, pageSize.SizeUnit);

                flowElement.PageWidth = pageWidth - paddingLeft - paddingRigt + RichTextBoxDefaultPadding;
                flowElement.PageHeight = pageHeight;
            }

            if (element.Blocks != null)
            {
                foreach (var block in element.Blocks)
                {
                    var flowBlock = context.Build<BlockElement>(block, documentMap);

                    if (flowBlock != null)
                    {
                        flowElement.Blocks.Add(flowBlock);
                    }
                }
            }

            return flowElement;
        }


        private static void ApplyDocumentStyles(FlowDocument flowElement, PrintDocument element)
        {
            FlowBuilderHelper.SetFont(flowElement, element.Font);
            FlowBuilderHelper.SetForeground(flowElement, element.Foreground);
            FlowBuilderHelper.SetBackground(flowElement, element.Background);
        }
    }
}