using System;

namespace DXFileExplorer.Utils {
    public class CreateNavItemArgs {
        public CreateNavItemArgs(string groupName, string itemName) {
            fGroupName = groupName;
            fItemName = itemName;
        }

        string fGroupName;
        public string GroupName {
            get { return fGroupName; }
        }

        string fItemName;
        public string ItemName {
            get { return fItemName; }
        }

        int fIndex = -1;
        public int Index {
            get { return fIndex; }
            set { fIndex = value; }
        }

        public string GroupText { get; set; }
        public string ItemText { get; set; }
        public Action<string> Callback { get; set; }
        public string Location { get; set; }
    }
}
