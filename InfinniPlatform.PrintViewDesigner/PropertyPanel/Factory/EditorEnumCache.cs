using System;
using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal class EditorEnumCache
    {
        private readonly Dictionary<Type, Dictionary<object, string>> _enums
            = new Dictionary<Type, Dictionary<object, string>>();


        public EditorEnumCache AddValue<TValue>(TValue value, string caption)
        {
            Dictionary<object, string> enumValues;

            if (!_enums.TryGetValue(typeof(TValue), out enumValues))
            {
                enumValues = new Dictionary<object, string>();

                _enums.Add(typeof(TValue), enumValues);
            }

            enumValues.Add(value, caption);

            return this;
        }


        public IDictionary<object, string> GetValues<TValue>()
        {
            Dictionary<object, string> enumValues;

            _enums.TryGetValue(typeof(TValue), out enumValues);

            return enumValues;
        }
    }
}