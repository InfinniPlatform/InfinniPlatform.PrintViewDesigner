using System.ComponentModel;
using System.Windows;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid
{
    /// <summary>
    /// Информация о свойстве.
    /// </summary>
    public sealed class PropertyDefinition : INotifyPropertyChanged
    {
        private Visibility _visibility;

        /// <summary>
        /// Видимость свойства.
        /// </summary>
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (!Equals(_visibility, value))
                {
                    _visibility = value;

                    OnPropertyChanged("Visibility");
                }
            }
        }


        private string _name;

        /// <summary>
        ///  Наименование свойства.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!Equals(_name, value))
                {
                    _name = value;

                    OnPropertyChanged("Name");
                }
            }
        }


        private string _caption;

        /// <summary>
        /// Заголовок свойства.
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set
            {
                if (!Equals(_caption, value))
                {
                    _caption = value;

                    OnPropertyChanged("Caption");
                }
            }
        }


        private PropertyEditorBase _editor;

        /// <summary>
        /// Редактор свойства.
        /// </summary>
        public PropertyEditorBase Editor
        {
            get { return _editor; }
            set
            {
                if (!Equals(_editor, value))
                {
                    _editor = value;

                    OnPropertyChanged("Editor");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}