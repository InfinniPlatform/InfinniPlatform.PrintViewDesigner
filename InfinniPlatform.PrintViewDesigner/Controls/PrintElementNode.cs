using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InfinniPlatform.PrintViewDesigner.Controls
{
    public sealed class PrintElementNode : INotifyPropertyChanged
    {
        private string _displayName;
        private int _index;
        private string _visibility;

        public PrintElementNode(PrintElementNode elementParent, string elementType, dynamic elementMetadata)
        {
            Key = this;
            Parent = elementParent;
            Nodes = new List<PrintElementNode>();

            ElementType = elementType;
            ElementMetadata = elementMetadata;
        }

        public int Index
        {
            get { return _index; }
            set
            {
                if (!Equals(_index, value))
                {
                    _index = value;

                    OnPropertyChanged("Index");
                }
            }
        }

        public string Visibility
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

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (!Equals(_displayName, value))
                {
                    _displayName = value;

                    OnPropertyChanged("DisplayName");
                }
            }
        }

        public PrintElementNode Key { get; private set; }
        public PrintElementNode Parent { get; private set; }
        public List<PrintElementNode> Nodes { get; private set; }
        public string ElementType { get; private set; }
        public dynamic ElementMetadata { get; private set; }
        public string[] ElementChildrenTypes { get; set; }
        public Func<string> BuildVisibility { get; set; }
        public Func<string> BuildDisplayName { get; set; }
        public Func<PrintElementNode, bool> CanInsertChild { get; set; }
        public Func<PrintElementNode, bool> InsertChild { get; set; }
        public Func<PrintElementNode, bool, bool> CanDeleteChild { get; set; }
        public Func<PrintElementNode, bool, bool> DeleteChild { get; set; }
        public Func<PrintElementNode, int, bool> CanMoveChild { get; set; }
        public Func<PrintElementNode, int, bool> MoveChild { get; set; }
        public Func<bool> CanCut { get; set; }
        public Func<bool> Cut { get; set; }
        public Func<bool> CanCopy { get; set; }
        public Func<bool> Copy { get; set; }
        public Func<bool> CanPaste { get; set; }
        public Func<bool> Paste { get; set; }
        // INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void Refresh()
        {
            Visibility = (BuildVisibility != null) ? BuildVisibility() : null;
            DisplayName = (BuildDisplayName != null) ? BuildDisplayName() : ElementType;
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}