using System;
using System.IO;
using System.Linq;
using DXFileExplorer.Models;
using DXFileExplorer.Extensions;
using System.Collections.Generic;

namespace DXFileExplorer {
    public static class DataLayer {
        public static IEnumerable<FileSystemItem> GetFileSystemItemsFromDirectory(string directory) {
            int cnt = 0;
            DirectoryInfo di = new DirectoryInfo(directory);
            List<FileSystemItem> result = new List<FileSystemItem>();
            try {
                result = (from fsi in di.GetFileSystemInfos()
                            select new FileSystemItem() {
                                ID = ++cnt, ItemType = (fsi.Attributes & FileAttributes.Directory) ==
                                FileAttributes.Directory ? FileSystemItemType.Directory :
                                FileSystemItemType.File, Name = fsi.Name, Size = (fsi.Attributes &
                                FileAttributes.Directory) == FileAttributes.Directory ? "Directory" :
                                GetFileSize(((FileInfo)fsi).Length), 
                                LastModifiedDate = fsi.LastWriteTime
                            }).ToList();
            } catch (SystemException ex) { //UnauthorizedAccessException, DirectoryNotFoundException
                result.Add(new FileSystemItem() { 
                    ID = ++cnt, ItemType = FileSystemItemType.Error, Name = ex.Message,
                    Size = "Error", LastModifiedDate = DateTime.Now
                });
            }
            if (di.Parent != null && di.Parent.Exists)
                result.Insert(0, new FileSystemItem() {
                    ID = ++cnt, ItemType = FileSystemItemType.RootDirectory, Name = "..",
                    Size = "Up", LastModifiedDate = di.LastWriteTime
                });
            return result;
        }

        static string GetFileSize(long size) {
            const int k = 0x400;
            const int m = 0x100000;
            const int g = 0x40000000;
            if (size < k) return size.ToString();
            if (size >= k && size < m) return string.Concat(size / k, " K");
            if (size >= m && size < g) return string.Concat(size / m, " M");
            return string.Concat(size / g, " G");
        }
    }
}
