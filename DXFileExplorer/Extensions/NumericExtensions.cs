using System;
using System.Collections.Generic;

namespace DXFileExplorer.Extensions {
    public static class NumericExtensions {
        public static IEnumerable<int> GetRange(this int start, int end) {
            for (int i = Math.Min(start, end); i < Math.Max(start, end) + 1; i++)
                yield return i;
        }
    }
}
