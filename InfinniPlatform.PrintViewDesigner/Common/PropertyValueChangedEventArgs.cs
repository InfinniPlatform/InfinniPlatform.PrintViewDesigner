using System.Windows;

namespace InfinniPlatform.PrintViewDesigner.Common
{
    public sealed class PropertyValueChangedEventArgs : RoutedEventArgs
    {
        public PropertyValueChangedEventArgs(RoutedEvent routedEvent, string property, object oldValue, object newValue)
            : base(routedEvent)
        {
            Property = property;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public string Property { get; }

        public object OldValue { get; }

        public object NewValue { get; }

        public PropertyValueChangedEventArgs Create(RoutedEvent routedEvent)
        {
            return new PropertyValueChangedEventArgs(routedEvent, Property, OldValue, NewValue);
        }
    }
}