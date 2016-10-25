using System;
using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory
{
    internal class FlowEnumCache
    {
        private readonly Dictionary<Type, Dictionary<object, object>> _enums
            = new Dictionary<Type, Dictionary<object, object>>();


        public FlowEnumCache AddValue(object key, object value)
        {
            Dictionary<object, object> enumValues;

            if (!_enums.TryGetValue(key.GetType(), out enumValues))
            {
                enumValues = new Dictionary<object, object>();

                _enums.Add(key.GetType(), enumValues);
            }

            enumValues.Add(key, value);

            return this;
        }


        public TValue GetValue<TValue>(object key, TValue defaultValue = default(TValue))
        {
            object value;

            Dictionary<object, object> values;

            return (key != null)
                   && _enums.TryGetValue(key.GetType(), out values)
                   && values.TryGetValue(key, out value)
                   && (value is TValue)
                ? (TValue)value
                : defaultValue;
        }
    }
}