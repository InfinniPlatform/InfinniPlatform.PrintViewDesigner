using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

using DevExpress.Xpf.Grid;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid
{
    /// <summary>
    /// Список свойств.
    /// </summary>
    public sealed class PropertyCollection : IEnumerable<PropertyDefinition>, INotifyCollectionChanged
    {
        private readonly TreeListNodeCollection _parentNodes;
        private readonly Dictionary<PropertyDefinition, TreeListNode> _properties;
        private readonly PropertyRegister _register;

        public PropertyCollection(PropertyRegister register, TreeListNodeCollection parentNodes)
        {
            if (register == null)
            {
                throw new ArgumentNullException(nameof(register));
            }

            if (parentNodes == null)
            {
                throw new ArgumentNullException(nameof(parentNodes));
            }

            _register = register;
            _parentNodes = parentNodes;
            _properties = new Dictionary<PropertyDefinition, TreeListNode>();
        }

        public IEnumerator<PropertyDefinition> GetEnumerator()
        {
            return _properties.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Добавляет свойство в список.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(PropertyDefinition property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!_properties.ContainsKey(property))
            {
                var node = new TreeListNode(property);

                if (property.Editor != null)
                {
                    property.Editor.Properties = new PropertyCollection(_register, node.Nodes);
                }

                _register.Register(property);

                if (property.Visibility == Visibility.Visible)
                {
                    _parentNodes.Add(node);
                }

                _properties.Add(property, node);

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, property));
            }
        }

        /// <summary>
        /// Удаляет свойство из списка.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public void Remove(PropertyDefinition property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            TreeListNode node;

            if (_properties.TryGetValue(property, out node))
            {
                _register.Unregister(property);

                _parentNodes.Remove(node);
                _properties.Remove(property);

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, property));
            }
        }

        /// <summary>
        /// Очищает список свойств.
        /// </summary>
        public void Clear()
        {
            if (_properties.Count > 0)
            {
                foreach (var item in _properties)
                {
                    _register.Unregister(item.Key);
                }

                _properties.Clear();
                _parentNodes.Clear();

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var handler = CollectionChanged;

            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}