using System;
using System.IO;
using DXFileExplorer.Properties;

namespace DXFileExplorer.Actions {
    public class ExecuteMoveEventArgs :ExecuteActionEventArgs {
        public override void Accept(IActionVisitor visitor) {
            visitor.Visit(this);
        }

        public override void DoOperation(FileSystemInfo source, string destination, Action<string, string> guiCallback) {
            destination = Path.Combine(destination, source.Name);
            if (source is DirectoryInfo) {
                guiCallback(Resources.Default_FileSystem_GuiCallback_Directory, source.Name);
                ((DirectoryInfo)source).MoveTo(destination);
            } else {
                guiCallback(Resources.Default_FileSystem_GuiCallback_File, source.Name);
                ((FileInfo)source).MoveTo(destination);
            }
        }
    }
}
