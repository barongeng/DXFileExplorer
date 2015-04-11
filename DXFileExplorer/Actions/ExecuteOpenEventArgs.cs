using DXFileExplorer.Models;

namespace DXFileExplorer.Actions {
    public class ExecuteOpenEventArgs :ExecuteActionEventArgs{
        ExecuteOpenEventArgs(string path) {
            fPath = path;
        }

        public ExecuteOpenEventArgs(string path, bool isDirectory) : this(path) {
            fIsDirectory = isDirectory;
            fNeedCombinePath = true;
        }

        public ExecuteOpenEventArgs(string path, string args) : this(path) {
            fArguments = args;
        }

        readonly string fPath;
        public string Path {
            get { return fPath; }
        }

        readonly string fArguments;
        public string Arguments {
            get { return fArguments; }
        }

        readonly bool fNeedCombinePath;
        public bool NeedCombinePath {
            get { return fNeedCombinePath; }
        }

        readonly bool fIsDirectory;
        public bool IsDirectory {
            get { return fIsDirectory; }
        }

        public override void Accept(IActionVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
