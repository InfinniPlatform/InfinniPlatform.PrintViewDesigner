using System;
using System.Collections.Generic;
using System.Windows;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid
{
    /// <summary>
    /// Регистр свойств.
    /// </summary>
    public sealed class PropertyRegister
    {
        private object _editValue;
        private bool _handleEvent;
        private readonly RoutedEvent _editValueChangedRoutedEvent;
        private readonly UIElement _editValueControl;
        private readonly List<PropertyDefinition> _properties = new List<PropertyDefinition>();

        public PropertyRegister(UIElement editValueControl, RoutedEvent editValueChangedRoutedEvent)
        {
            if (editValueControl == null)
            {
                throw new ArgumentNullException(nameof(editValueControl));
            }

            if (editValueChangedRoutedEvent == null)
            {
                throw new ArgumentNullException(nameof(editValueChangedRoutedEvent));
            }

            _editValueControl = editValueControl;
            _editValueChangedRoutedEvent = editValueChangedRoutedEvent;
        }

        /// <summary>
        /// Редактируемое значение.
        /// </summary>
        public object EditValue
        {
            get
            {
                return _editValue;
            }
            set
            {
                var oldValue = _editValue;

                if (!Equals(oldValue, value))
                {
                    _editValue = value;

                    OnSetEditValueHandler(oldValue, value);
                }
            }
        }

        /// <summary>
        /// Добавляет свойство в регистр.
        /// </summary>
        public void Register(PropertyDefinition property)
        {
            if (!_properties.Contains(property))
            {
                _properties.Add(property);

                if (property.Editor != null)
                {
                    property.Editor.EditValueChanged += OnSetPropertyValueHandler;
                }
            }
        }

        /// <summary>
        /// Удаляет свойство из регистра.
        /// </summary>
        public void Unregister(PropertyDefinition property)
        {
            if (_properties.Remove(property))
            {
                if (property.Editor != null)
                {
                    property.Editor.EditValueChanged -= OnSetPropertyValueHandler;

                    if (property.Editor.Properties != null)
                    {
                        property.Editor.Properties.Clear();
                    }
                }
            }
        }

        private void OnSetEditValueHandler(object oldValue, object newValue)
        {
            if (!_handleEvent)
            {
                _handleEvent = true;

                try
                {
                    foreach (var property in _properties)
                    {
                        if (property.Editor != null)
                        {
                            var propertyValue = EditValue.GetProperty(property.Name);

                            property.Editor.EditValue = propertyValue;
                        }
                    }

                    OnEditValueChanged(null, oldValue, newValue);
                }
                finally
                {
                    _handleEvent = false;
                }
            }
        }

        private void OnSetPropertyValueHandler(object sender, PropertyValueChangedEventArgs e)
        {
            if (!_handleEvent)
            {
                _handleEvent = true;

                try
                {
                    if (EditValue != null)
                    {
                        EditValue.SetProperty(e.Property, e.NewValue);

                        foreach (var property in _properties)
                        {
                            if (property.Editor != null)
                            {
                                if (property.Name == e.Property || property.Name.StartsWith(e.Property + "."))
                                {
                                    var propertyValue = EditValue.GetProperty(property.Name);

                                    property.Editor.EditValue = propertyValue;
                                }
                            }
                        }

                        OnEditValueChanged(e.Property, e.OldValue, e.NewValue);
                    }
                }
                finally
                {
                    _handleEvent = false;
                }
            }
        }

        private void OnEditValueChanged(string property, object oldValue, object newValue)
        {
            _editValueControl.RaiseEvent(new PropertyValueChangedEventArgs(_editValueChangedRoutedEvent, property, oldValue, newValue));
        }
    }
}