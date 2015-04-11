namespace DXFileExplorer.Views.Collections {
    public class FileSystemViewDataSource {
        readonly FileSystemViewData fDataA = new FileSystemViewData();
        public FileSystemViewData DataA {
            get { return fDataA; }
        }

        readonly FileSystemViewData fDataB = new FileSystemViewData();
        public FileSystemViewData DataB {
            get { return fDataB; }
        }
    }
}
