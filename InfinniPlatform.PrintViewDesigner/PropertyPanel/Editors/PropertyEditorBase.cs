using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using InfinniPlatform.PrintViewDesigner.Common;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors
{
    /// <summary>
    /// Редактор свойства объекта.
    /// </summary>
    public class PropertyEditorBase : UserControl
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PropertyEditorBase()
        {
            SetEditValueBinding();
        }


        /// <summary>
        /// Идентификатор редактора для отображения в дереве.
        /// </summary>
        public object TreeKey => this;

        /// <summary>
        /// Идентификатор родительского редактора для отображения в дереве.
        /// </summary>
        public object TreeParent { get; private set; }


        // Property

        public static readonly DependencyProperty PropertyProperty
            = DependencyProperty.Register(nameof(Property),
                                          typeof(string),
                                          typeof(PropertyEditorBase),
                                          new PropertyMetadata(OnPropertyChanged));

        /// <summary>
        /// Свойство.
        /// </summary>
        public string Property
        {
            get { return (string)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var editor = (PropertyEditorBase)sender;

            var newProperty = (string)e.NewValue;

            editor.SetEditValueBinding(newProperty);
        }

        private void SetEditValueBinding(string property = null)
        {
            var propertyBinding = new Binding(property);

            if (!string.IsNullOrEmpty(property))
            {
                propertyBinding.Mode = BindingMode.TwoWay;
            }

            BindingOperations.ClearBinding(this, EditValueProperty);

            BindingOperations.SetBinding(this, EditValueProperty, propertyBinding);
        }


        // Caption

        public static readonly DependencyProperty CaptionProperty
            = DependencyProperty.Register(nameof(Caption),
                                          typeof(string),
                                          typeof(PropertyEditorBase));

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }


        // Editors

        /// <summary>
        /// Добавляет редактор дочернего свойства.
        /// </summary>
        public void AddEditor(ICollection<PropertyEditorBase> editors, PropertyEditorBase editor)
        {
            // Текущий редактор является родительским
            editor.TreeParent = this;

            // Подписка на изменение значения дочернего свойства
            editor.EditValueChanged += OnChildEditValueChanged;

            // Значение текущего редактора является источником данных
            var editValueBinding = new Binding(nameof(EditValue)) { Source = this, Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(editor, DataContextProperty, editValueBinding);

            // Если элемент не является дочерним (не часть сложного редактора)
            if (editors != null && !ContainsEditor(editor))
            {
                editors.Add(editor);
            }
        }

        private bool ContainsEditor(FrameworkElement child)
        {
            var result = false;

            while (child != null && !(result = ReferenceEquals(child.Parent, this)))
            {
                child = child.Parent as FrameworkElement;
            }

            return result;
        }


        // Categories

        private readonly Dictionary<string, PropertyEditorBase> _categories
            = new Dictionary<string, PropertyEditorBase>();

        /// <summary>
        /// Добавляет категорию редакторов дочерних свойств.
        /// </summary>
        public PropertyEditorBase AddCategory(ICollection<PropertyEditorBase> editors, string caption)
        {
            PropertyEditorBase category;

            if (!_categories.TryGetValue(caption, out category))
            {
                category = new PropertyEditorBase { Caption = caption };

                _categories.Add(caption, category);

                AddEditor(editors, category);
            }

            return category;
        }


        // EditValue

        public static readonly DependencyProperty EditValueProperty
            = DependencyProperty.Register(nameof(EditValue),
                                          typeof(object),
                                          typeof(PropertyEditorBase),
                                          new FrameworkPropertyMetadata(OnEditValueChanged));

        /// <summary>
        /// Значение свойства.
        /// </summary>
        public object EditValue
        {
            get { return GetValue(EditValueProperty); }
            set { SetValue(EditValueProperty, value); }
        }


        /// <summary>
        /// Обрабатывает изменение <see cref="EditValue"/>.
        /// </summary>
        private static void OnEditValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var editor = (PropertyEditorBase)sender;

            editor.InvokeEditValueChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// Обрабатывает изменение <see cref="EditValue"/> дочернего свойства.
        /// </summary>
        private void OnChildEditValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (TryBeginEditValueChanging())
            {
                try
                {
                    // Если значение не задано
                    if (EditValue == null && e.NewValue != null)
                    {
                        var childEditor = (PropertyEditorBase)sender;

                        // Оно создается автоматически
                        var newEditValue = CreateNewEditValue?.Invoke();

                        if (newEditValue != null)
                        {
                            if (!string.IsNullOrEmpty(childEditor.Property))
                            {
                                // И инициализируется значением дочернего свойства
                                newEditValue.SetProperty(childEditor.Property, e.NewValue);
                            }

                            EditValue = newEditValue;
                        }
                    }
                }
                finally
                {
                    EndEditValueChanging();
                }
            }
        }


        private bool _changing;

        /// <summary>
        /// Начинает обработку изменения <see cref="EditValue"/>.
        /// </summary>
        private bool TryBeginEditValueChanging()
        {
            if (!_changing)
            {
                _changing = true;
            }

            return _changing;
        }

        /// <summary>
        /// Заканчивает обработку изменения <see cref="EditValue"/>.
        /// </summary>
        private void EndEditValueChanging()
        {
            _changing = false;
        }


        // EditValueChanged

        public static readonly RoutedEvent EditValueChangedEvent
            = EventManager.RegisterRoutedEvent(nameof(EditValueChanged),
                                               RoutingStrategy.Bubble,
                                               typeof(PropertyValueChangedEventHandler),
                                               typeof(PropertyEditorBase));

        /// <summary>
        /// Событие изменения значения свойства <see cref="EditValue"/>.
        /// </summary>
        public event PropertyValueChangedEventHandler EditValueChanged
        {
            add { AddHandler(EditValueChangedEvent, value); }
            remove { RemoveHandler(EditValueChangedEvent, value); }
        }

        private void InvokeEditValueChanged(object oldValue, object newValue)
        {
            RaiseEvent(new PropertyValueChangedEventArgs(EditValueChangedEvent, Property, oldValue, newValue));
        }


        /// <summary>
        /// Создает новый экземпляр для значения свойства <see cref="EditValue"/>.
        /// </summary>
        public Func<object> CreateNewEditValue { get; set; }
    }
}