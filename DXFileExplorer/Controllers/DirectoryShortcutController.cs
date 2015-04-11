using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DXFileExplorer.Properties;
using DXFileExplorer.Utils;

namespace DXFileExplorer.Controllers {
    public class DirectoryShortcutController :BaseController {
        static readonly Keys[] Shortcuts = new Keys[] { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
        static readonly string[] Directories = new string[10];

        protected override string SettingsName {
            get { return "Default_DirectoryShortcut"; }
        }

        public DirectoryShortcutController(IControllerManager manager) : base(manager) {
            for (int i = 0; i < 10; i++) {
                manager.AddCommand(new CreateCommandArgs() {
                    PageText = string.Empty,
                    GroupText = string.Empty,
                    Info = new CommandInfo() {
                        CommandName = string.Concat("Default_DirectoryShortcut_Create", i),
                        CommandText = string.Empty,
                        Key = Keys.Control | Keys.Shift | Shortcuts[i],
                        Callback = t => CreateShortcut(Convert.ToInt32(t), Manager.CurrentDirectory),
                        Tag = i
                    }
                });
            }
        }

        void CreateShortcut(int i, string directory) {
            string commandText = i.ToString();
            Manager.AddCommand(new CreateCommandArgs() {
                PageText = Resources.NavigationPageText,
                GroupText = Resources.NavigationPageShortcutsGroupText,
                Info = new CommandInfo() {
                    CommandName = string.Concat("Default_DirectoryShortcut_", commandText),
                    CommandText = commandText,
                    Key = Keys.Control | Shortcuts[i],
                    Callback = t => Manager.CurrentDirectory = Directories[(int)t],
                    Tag = i, Tooltip = directory
                }
            });
            Directories[i] = directory;
        }

        protected override void Dispose(bool disposing) {
            List<string> settings = new List<string>();
            for (int i = 0; i < Directories.Length; i++) {
                string directory = Directories[i];
                if (!string.IsNullOrEmpty(directory))
                    settings.Add(string.Concat(i, SettingInnerSeparator, directory));
            }
            WriteApplicationSetting(string.Join<string>(SettingOuterSeparator.ToString(), settings));
            base.Dispose(disposing);
        }

        protected override void OnCurrentDirectoryChanged() { }

        protected override void RestoreSetting(params string[] args) {
            CreateShortcut(int.Parse(args[0]), args[1]);
        }
    }
}
