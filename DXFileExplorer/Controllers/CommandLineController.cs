using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using DXFileExplorer.Properties;
using DXFileExplorer.Views;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using DXFileExplorer.Utils;
using DXFileExplorer.Models;

namespace DXFileExplorer.Controllers {
    public class CommandLineController :BaseController {
        const string PaneName = "Default_CommandLine_Console";
        const string ViewCommandName = "Default_CommandLine_Show";
        const string CopyPathCommandName = "Default_CommandLine_CopyPath";
        CommandLineView View;
        Guid PaneId = new Guid(new byte[] { 0xca, 0xe3, 0x04, 0x6c, 0x6f, 0x8d, 0x4d, 0x3f, 0x81, 0x59, 0xff, 0xaa, 0xb1, 0x10, 0x45, 0x91 });
                
        public CommandLineController(IControllerManager manager) : base(manager) {
            View = new CommandLineView() {
                Name = PaneName, Dock = DockStyle.Fill,
                Text = Resources.ConsoleCaption
            };
            View.ExecuteCommand += (s, e) => {
                string[] keys = e.Command.Split(' ');
                switch (keys[0]) {
                    case "cd": 
                        Manager.CurrentDirectory = keys[1];
                        Manager.ActivateFileSystemView();
                        break;
                    default:
                        Manager.ExecuteFile(keys[0], string.Join(" ", keys.Skip(1)));
                        break;
                }
                View.DisplayPrompt(Manager.CurrentDirectory, true);
            };
            Manager.AddPane(View, PaneDockingStyle.Bottom, PaneId);
            Manager.AddCommand(new CreateCommandArgs() {
                Info = new CommandInfo() {
                    Callback = o => {
                        if (View.Focused)
                            Manager.ActivateFileSystemView();
                        else View.Focus();
                    }, CommandName = ViewCommandName,
                    Key = Keys.Control | Keys.Shift | Keys.C
                }
            });
            View.DisplayPrompt(Manager.CurrentDirectory, false);
            Manager.AddCommand(new CreateCommandArgs() {
                Info = new CommandInfo() {
                    Callback = o => {
                        string[] paths = Manager.GetSelectedFiles(
                        ).Select(i => Path.Combine(Manager.CurrentDirectory, i.ItemType
                            == FileSystemItemType.RootDirectory ? string.Empty : i.Name)).ToArray();
                        if (paths.Length > 0)
                            Clipboard.SetText(string.Join("\r\n", paths));
                    },
                    CommandName = CopyPathCommandName,
                    Key = Keys.Control | Keys.F
                }
            });
        }

        protected override string SettingsName {
            get { return "Default_CommandLine"; }
        }

        protected override void Dispose(bool disposing) {
            Manager.RemovePane(PaneName);
            Manager.RemoveCommand(ViewCommandName);
            View = null;
            base.Dispose(disposing);
        }

        protected override void OnCurrentDirectoryChanged() {
            View.DisplayPrompt(Manager.CurrentDirectory, false);
        }

        protected override void RestoreSetting(params string[] args) { }
    }
}
