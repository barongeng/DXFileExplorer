using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DXFileExplorer.Models;
using DXFileExplorer.Utils;

namespace DXFileExplorer.Controllers {
    public interface IControllerManager {
        bool AddCommand(CreateCommandArgs args);
        void RemoveCommand(string commandName);
        string ReadApplicationSetting(string name);
        void WriteApplicationSetting(string name, string value);
        string CurrentDirectory { get; set; }
        void AddPane(Control content, PaneDockingStyle dock, Guid id);
        void RemovePane(string name);
        event EventHandler CurrentDirectoryChanged;
        void ActivateFileSystemView();
        void ExecuteFile(string file, string args);
        void AddNavItem(CreateNavItemArgs args);
        void RemoveNavItem(string name);
        void ShowDialog(Form dialog);
        void ShowError(string message, MessageBoxButtons buttons, MessageBoxIcon icon);
        IEnumerable<FileSystemItem> GetSelectedFiles();
        void ShowView(Control view);
        void SetItemEnabled(string name, bool enabled);
        void ChangeItemCaption(string name, string caption);
    }
}
