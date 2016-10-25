using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Block;

using BlockElement = System.Windows.Documents.Block;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Block
{
    internal sealed class PrintSectionFlowBuilder : FlowBuilderBase<PrintSection>
    {
        protected override object Build(FlowBuilderContext context, PrintSection element, PrintDocumentMap documentMap)
        {
            var flowElement = new Section();

            FlowBuilderHelper.ApplyElementStyles(flowElement, element);
            FlowBuilderHelper.ApplyBlockStyles(flowElement, element);

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
    }
}