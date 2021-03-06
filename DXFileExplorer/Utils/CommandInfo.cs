﻿using System;
using System.Windows.Forms;

namespace DXFileExplorer.Utils {
    public class CommandInfo {
        public string CommandText { get; set; }
        public string CommandName { get; set; }
        public Action<object> Callback { get; set; }
        public Keys Key { get; set; }
        public Keys SecondKey { get; set; }
        public object Tag { get; set; }
        public string Tooltip { get; set; }
    }
}
