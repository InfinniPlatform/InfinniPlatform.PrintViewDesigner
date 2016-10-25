using InfinniPlatform.PrintView.Contract;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory
{
    internal interface IFlowBuilder
    {
        object Build(FlowBuilderContext context, object element, PrintDocumentMap documentMap);
    }
}