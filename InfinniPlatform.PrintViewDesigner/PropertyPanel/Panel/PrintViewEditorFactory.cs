using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Block;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory.Inline;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Panel
{
    /// <summary>
    /// Фабрика создания редактора свойств для заданного типа элементов печатного представления.
    /// </summary>
    internal class PrintViewEditorFactory
    {
        public PrintViewEditorFactory(Func<PrintDocument> template)
        {
            _template = template;
            _context = new EditorBuilderContext();
            _editors = new Dictionary<Type, Lazy<PropertyEditorBase>>();

            RegisterFactory(new PrintBorderEditorFactory());
            RegisterFactory(new PrintDocumentEditorFactory());
            RegisterFactory(new PrintFontEditorFactory());
            RegisterFactory(new PrintSizeEditorFactory());
            RegisterFactory(new PrintStyleEditorFactory());
            RegisterFactory(new PrintThicknessEditorFactory());

            // Bock
            RegisterFactory(new PrintLineEditorFactory());
            RegisterFactory(new PrintListEditorFactory());
            RegisterFactory(new PrintPageBreakBreakEditorFactory());
            RegisterFactory(new PrintParagraphEditorFactory());
            RegisterFactory(new PrintSectionEditorFactory());
            RegisterFactory(new PrintTableCellEditorFactory());
            RegisterFactory(new PrintTableColumnEditorFactory());
            RegisterFactory(new PrintTableEditorFactory());
            RegisterFactory(new PrintTableRowEditorFactory());

            // Inline
            RegisterFactory(new PrintBarcodeEan13EditorFactory());
            RegisterFactory(new PrintBarcodeQrEditorFactory());
            RegisterFactory(new PrintBoldEditorFactory());
            RegisterFactory(new PrintHyperlinkEditorFactory());
            RegisterFactory(new PrintImageEditorFactory());
            RegisterFactory(new PrintItalicEditorFactory());
            RegisterFactory(new PrintLineBreakEditorFactory());
            RegisterFactory(new PrintRunEditorFactory());
            RegisterFactory(new PrintSpanEditorFactory());
            RegisterFactory(new PrintUnderlineEditorFactory());
        }


        private readonly Func<PrintDocument> _template;
        private readonly EditorBuilderContext _context;
        private readonly Dictionary<Type, Lazy<PropertyEditorBase>> _editors;


        private void RegisterFactory<T>(EditorFactoryBase<T> factory) where T : new()
        {
            var builder = new EditorBuilder<T>(_template, _context);
            factory.Builder = builder;

            _editors.Add(typeof(T), new Lazy<PropertyEditorBase>(CreateEditor<T>));

            _context.Register(factory);
        }

        private PropertyEditorBase CreateEditor<T>() where T : new()
        {
            // Список редакторов свойств, упорядоченных в иерархическом порядке
            var editors = new ObservableCollection<PropertyEditorBase>();

            // Создание редактора для заданного типа
            var editor = _context.Create<T>(editors);

            var propertyEditor = editor as PropertyEditor;

            // Если это корневой элемент
            if (propertyEditor != null)
            {
                // Он отображает дерево редакторов
                propertyEditor.Editors = editors;
            }

            return editor;
        }


        /// <summary>
        /// Создает редактор свойств для заданного типа элементов печатного представления.
        /// </summary>
        /// <param name="type">Тип элемента печатного представления.</param>
        public PropertyEditorBase CreateEditor(Type type)
        {
            Lazy<PropertyEditorBase> editor;

            return (_editors.TryGetValue(type, out editor)) ? editor.Value : null;
        }
    }
}