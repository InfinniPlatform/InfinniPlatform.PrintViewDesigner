using System.Windows;

namespace InfinniPlatform.PrintViewDesigner.Controls
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

        public string Property { get; private set; }
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }

        public PropertyValueChangedEventArgs Create(RoutedEvent routedEvent)
        {
            return new PropertyValueChangedEventArgs(routedEvent, Property, OldValue, NewValue);
        }
    }
}