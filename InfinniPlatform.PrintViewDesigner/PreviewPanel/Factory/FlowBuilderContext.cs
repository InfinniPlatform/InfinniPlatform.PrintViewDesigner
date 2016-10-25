using System;
using System.Collections.Generic;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory
{
    internal sealed class FlowBuilderContext
    {
        private readonly Dictionary<Type, IFlowBuilder> _builders
            = new Dictionary<Type, IFlowBuilder>();


        public void Register<TElement>(FlowBuilderBase<TElement> builder) where TElement : PrintElement
        {
            _builders[typeof(TElement)] = builder;
        }


        public TResult Build<TResult>(PrintElement element, PrintDocumentMap documentMap)
        {
            if (element != null)
            {
                IFlowBuilder builder;

                if (_builders.TryGetValue(element.GetType(), out builder))
                {
                    var flowElement = builder.Build(this, element, documentMap);

                    documentMap.RemapElement(element, flowElement);

                    return (TResult)flowElement;
                }
            }

            return default(TResult);
        }
    }
}