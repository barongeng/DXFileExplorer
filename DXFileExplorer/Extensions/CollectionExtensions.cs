using System.Linq;
using DXFileExplorer.Models;
using System.Collections.Generic;

namespace DXFileExplorer.Extensions {
    public static class IListExtensions {
        public static int GetNextID(this IList<FileSystemItem> source) {
            return source.Count == 0 ? 1 : source.Max(i => i.ID) + 1;
        }
    }
}
