using System;

namespace DXFileExplorer.Utils {
    public class ExecuteCommandEventArgs :EventArgs {
        public ExecuteCommandEventArgs(string command) {
            fCommand = command;
        }

        string fCommand;
        public string Command {
            get { return fCommand; }
        }
    }
}
