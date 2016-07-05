using System.Windows;
using System.Windows.Controls;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid
{
    /// <summary>
    /// Редактор свойств объекта.
    /// </summary>
    public sealed partial class PropertyGridControl : UserControl
    {
        private readonly PropertyRegister _propertyRegister;

        public PropertyGridControl()
        {
            InitializeComponent();

            _propertyRegister = new PropertyRegister(this, EditValueChangedEvent);

            Properties = new PropertyCollection(_propertyRegister, TreeList.View.Nodes);
        }

        // Properties

        public static readonly DependencyProperty PropertiesProperty
            = DependencyProperty.Register("Properties", typeof(PropertyCollection), typeof(PropertyGridControl));

        /// <summary>
        /// Список свойств.
        /// </summary>
        public PropertyCollection Properties
        {
            get { return (PropertyCollection)GetValue(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        // EditValue

        public static readonly DependencyProperty EditValueProperty
            = DependencyProperty.Register("EditValue", typeof(object), typeof(PropertyGridControl), new FrameworkPropertyMetadata(OnEditValueChanged));

        /// <summary>
        /// Редактируемый объект.
        /// </summary>
        public object EditValue
        {
            get { return GetValue(EditValueProperty); }
            set { SetValue(EditValueProperty, value); }
        }

        private static void OnEditValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PropertyGridControl;

            if (control != null)
            {
                control._propertyRegister.EditValue = e.NewValue;
            }
        }

        // EditValueChanged

        public static readonly RoutedEvent EditValueChangedEvent
            = EventManager.RegisterRoutedEvent("EditValueChanged", RoutingStrategy.Bubble, typeof(PropertyValueChangedEventHandler), typeof(PropertyGridControl));

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