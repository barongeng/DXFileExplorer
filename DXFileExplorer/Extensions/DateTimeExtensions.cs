using System;

namespace DXFileExplorer.Extensions {
    public static class DateTimeExtensions {
        public static string GUIToString(this DateTime date) {
            return date.ToString("g");
        }
    }
}
