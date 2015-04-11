using System;
using System.IO;
using DXFileExplorer.Properties;

namespace DXFileExplorer.Actions {
    public class ExecuteDeleteEventArgs :ExecuteActionEventArgs {
        public override void Accept(IActionVisitor visitor) {
            visitor.Visit(this);
        }

        public override void DoOperation(FileSystemInfo source, string destination, Action<string, string> guiCallback) {
            DirectoryInfo dir = source as DirectoryInfo;
            if (dir != null) {
                foreach (FileSystemInfo fi in dir.GetFileSystemInfos())
                    DoOperation(fi, dir.FullName, guiCallback);
                Directory.Delete(source.FullName);
            } else {
                guiCallback(Resources.Default_FileSystem_GuiCallback_File, source.Name);
                File.Delete(source.FullName);
            }
        }
    }
}
