using System;
using System.IO;
using System.Windows.Forms;
using DXFileExplorer.Properties;

namespace DXFileExplorer.Actions {
    public class ExecuteCopyEventArgs :ExecuteActionEventArgs {
        public override void Accept(IActionVisitor visitor) {
            visitor.Visit(this);
        }

        public override MessageBoxButtons ErrorDialogButtons {
            get { return MessageBoxButtons.YesNoCancel; }
        }

        bool fOverwrite;
        public bool Overwrite {
            get { return fOverwrite; }
        }

        public override bool? ProcessDialogResult(DialogResult result) {
            switch (result) {
                case DialogResult.Yes:
                    fOverwrite = true;
                    return null;
                case DialogResult.No: return false;
                case DialogResult.Cancel: return true;
                default: return ThrowException();
            }
        }

        public override void DoOperation(FileSystemInfo source, string destination, Action<string, string> guiCallback) {
            DirectoryInfo di = source as DirectoryInfo;
            string dest = Path.Combine(destination, source.Name);
            if (di == null) {
                guiCallback(Resources.Default_FileSystem_GuiCallback_Directory, source.Name);
                File.Copy(source.FullName, dest, Overwrite);
            } else {
                Directory.CreateDirectory(dest);
                foreach (FileSystemInfo fsi in di.GetFileSystemInfos())
                    DoOperation(fsi, dest, guiCallback);
            }
        }

        public override string GetExceptionMessage(string message) {
            return string.Concat(base.GetExceptionMessage(message), "\r\n", Resources.Default_FileSystem_Copy_Continue);
        }
    }
}
