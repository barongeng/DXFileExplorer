using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using DXFileExplorer.Properties;

namespace DXFileExplorer.Actions {
    public abstract class ExecuteActionEventArgs :EventArgs {
        public abstract void Accept(IActionVisitor visitor);

        public virtual MessageBoxButtons ErrorDialogButtons {
            get { return MessageBoxButtons.AbortRetryIgnore; }
        }

        public virtual bool? ProcessDialogResult(DialogResult result) {
            switch (result) {
                case DialogResult.Abort: return true;
                case DialogResult.Ignore: return false;
                case DialogResult.Retry: return null;
                default: return ThrowException();
            }
        }

        protected bool ThrowException() {
            throw new NotSupportedException(Resources.Default_FileSystem_UnexpectedDialogResult);
        }

        public virtual void DoOperation(FileSystemInfo source, string destination, Action<string, string> guiCallback) {
            throw new NotSupportedException(Resources.Default_FileSystem_UnsupportedOperation);
        }

        public virtual string GetExceptionMessage(string message) {
            return message;
        }
    }

    public interface IActionVisitor {
        void Visit(ExecuteOpenEventArgs e);
        void Visit(ExecuteCopyEventArgs e);
        void Visit(ExecuteMoveEventArgs e);
        void Visit(ExecuteMakeDirectoryEventArgs e);
        void Visit(ExecuteRenameEventArgs e);
        void Visit(ExecuteDeleteEventArgs e);
        void Visit(ExecuteRefreshEventArgs e);
    }
}
