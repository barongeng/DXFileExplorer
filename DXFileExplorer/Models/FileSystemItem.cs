using System;

namespace DXFileExplorer.Models {
    public class FileSystemItem {
        public int ID { get; set; }
        public string Name { get; set; }
        public FileSystemItemType ItemType { get; set; }
        public string Size { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public override string ToString() {
            return Name;
        }
    }

    public enum FileSystemItemType { RootDirectory, Directory, File, Error }
}
