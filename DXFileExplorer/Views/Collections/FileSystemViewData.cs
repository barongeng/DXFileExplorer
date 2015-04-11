using DXFileExplorer.Models;
using System.ComponentModel;

namespace DXFileExplorer.Views.Collections {
    public class FileSystemViewData : INotifyPropertyChanged {
        private string fPath;
        public string Path {
            get { return fPath; }
            set { SetPropertyValue<string>("Path", ref fPath, value); }
        }

        readonly BindingList<FileSystemItem> fSource = new BindingList<FileSystemItem>();
        public BindingList<FileSystemItem> Source {
            get { return fSource; }
        }

        void RaisePropertyChanged(string propertyName) {
            if (fPropertyChanged != null)
                fPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        void SetPropertyValue<T>(string propertyName, ref T valueHolder, T value) {
            if (Equals(valueHolder, value))
                return;
            valueHolder = value;
            RaisePropertyChanged(propertyName);
        }

        PropertyChangedEventHandler fPropertyChanged;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add { fPropertyChanged += value; }
            remove { fPropertyChanged -= value; }
        }
    }
}
