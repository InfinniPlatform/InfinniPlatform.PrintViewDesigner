using System.Windows.Documents;
using System.Windows.Media;

using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Views;

using FrameworkFlowDocument = System.Windows.Documents.FlowDocument;

namespace InfinniPlatform.PrintViewDesigner.ViewModel
{
    internal sealed class FlowElementFlowDocumentBuilder : IFlowElementBuilderBase<PrintViewDocument>
    {

        public override object Build(FlowElementBuilderContext context, PrintViewDocument element, PrintElementMetadataMap elementMetadataMap)
        {
            var elementContent = new FrameworkFlowDocument
            {
                FontFamily = new FontFamily("Arial")
            };

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyDocumentStyles(elementContent, element);

            foreach (var block in element.Blocks)
            {
                var blockContent = context.Build<Block>(block, elementMetadataMap);

                elementContent.Blocks.Add(blockContent);
            }

            return elementContent;
        }
    }
}
