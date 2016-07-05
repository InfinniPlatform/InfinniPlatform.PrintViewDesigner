using System.Windows;
using System.Windows.Controls;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid
{
    /// <summary>
    /// Редактор свойства.
    /// </summary>
    public class PropertyEditorBase : UserControl
    {
        protected void AddProperty(string propertyName, string propertyCaption, PropertyEditorBase propertyEditor, Visibility propertyVisibility)
        {
            propertyEditor.Property = Helpers.CombineProperties(Property, propertyName);

            Properties.Add(new PropertyDefinition
            {
                Name = propertyEditor.Property,
                Caption = propertyCaption,
                Editor = propertyEditor,
                Visibility = propertyVisibility
            });
        }

        protected virtual void OnPropertiesChanged(PropertyCollection properties)
        {
        }

        // Property

        public static readonly DependencyProperty PropertyProperty
            = DependencyProperty.Register("Property", typeof(string), typeof(PropertyEditorBase));

        /// <summary>
        /// Наименование свойства.
        /// </summary>
        public string Property
        {
            get { return (string)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        // Properties

        public static readonly DependencyProperty PropertiesProperty
            = DependencyProperty.Register("Properties", typeof(PropertyCollection), typeof(PropertyEditorBase), new PropertyMetadata(OnPropertiesChangedHandler));

        /// <summary>
        /// Список дочерних свойств.
        /// </summary>
        public PropertyCollection Properties
        {
            get { return (PropertyCollection)GetValue(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        private static void OnPropertiesChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PropertyEditorBase;

            if (control != null)
            {
                control.OnPropertiesChanged(e.NewValue as PropertyCollection);
            }
        }

        // EditValue

        public static readonly DependencyProperty EditValueProperty
            = DependencyProperty.Register("EditValue", typeof(object), typeof(PropertyEditorBase), new FrameworkPropertyMetadata(OnEditValueChanged));

        /// <summary>
        /// Значение свойства.
        /// </summary>
        public object EditValue
        {
            get { return GetValue(EditValueProperty); }
            set { SetValue(EditValueProperty, value); }
        }

        private static void OnEditValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PropertyEditorBase;

            if (control != null)
            {
                control.RaiseEvent(new PropertyValueChangedEventArgs(EditValueChangedEvent, control.Property, e.OldValue, e.NewValue));
            }
        }

        // EditValueChanged

        public static readonly RoutedEvent EditValueChangedEvent
            = EventManager.RegisterRoutedEvent("EditValueChanged", RoutingStrategy.Bubble, typeof(PropertyValueChangedEventHandler), typeof(PropertyEditorBase));

        /// <summary>
        /// Событие изменения значения свойства.
        /// </summary>
        public event PropertyValueChangedEventHandler EditValueChanged
        {
            add { AddHandler(EditValueChangedEvent, value); }
            remove { RemoveHandler(EditValueChangedEvent, value); }
        }
    }
}