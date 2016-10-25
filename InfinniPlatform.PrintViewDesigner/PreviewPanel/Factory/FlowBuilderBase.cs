using InfinniPlatform.PrintView.Contract;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory
{
    internal abstract class FlowBuilderBase<TElement> : IFlowBuilder
    {
        public object Build(FlowBuilderContext context, object element, PrintDocumentMap documentMap)
        {
            return Build(context, (TElement)element, documentMap);
        }

        protected abstract object Build(FlowBuilderContext context, TElement element, PrintDocumentMap documentMap);
    }
}