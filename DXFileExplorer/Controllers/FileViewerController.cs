using System.IO;
using System.Windows.Forms;
using System.Linq;
using DevExpress.XtraEditors;
using DXFileExplorer.Properties;
using DXFileExplorer.Utils;
using System;

namespace DXFileExplorer.Controllers {
    public class FileViewerController :BaseController {
        const string ItemViewName = "Default_Viewer_View";
        const string ItemEditName = "Default_Viewer_Edit";
        const string ItemSaveName = "Default_Viewer_Save";

        public FileViewerController(IControllerManager manager) :base(manager) {
            Manager.AddCommand(new CreateCommandArgs() {
                GroupText = Resources.Default_Viewer_GroupText, PageText = EditPageText,
                Info = new CommandInfo() {
                    CommandName = ItemViewName, CommandText = Resources.Default_Viewer_Command_View,
                    Key = Keys.F3,
                    Callback = o => ShowView(true)
                }
            });
            Manager.AddCommand(new CreateCommandArgs() {
                GroupText = Resources.Default_Viewer_GroupText, PageText = EditPageText,
                Info = new CommandInfo() {
                    CommandName = ItemEditName, CommandText = Resources.Default_Viewer_Command_Edit,
                    Key = Keys.F4,
                    Callback = o => ShowView(false)
                }
            });
            Manager.AddCommand(new CreateCommandArgs() {
                GroupText = Resources.Default_Viewer_GroupText, PageText = EditPageText,
                Info = new CommandInfo() {
                    CommandName = ItemSaveName, CommandText = Resources.Default_Viewer_Command_Save,
                    Key = Keys.F2,
                    Callback = ob => {
                        try {
                            File.WriteAllText(GetPath(), View.Text);
                        } catch (SystemException ex) {
                            Manager.ShowError(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            });
            Manager.SetItemEnabled(ItemSaveName, false);
        }

        protected override string SettingsName {
            get { return "Default_Viewer"; }
        }

        BaseEdit fView;
        BaseEdit View {
            get {
                if (fView == null) {
                    fView = new MemoEdit() { Dock = DockStyle.Fill };
                    fView.ParentChanged += (s, e) => {
                        if (fView.Parent == null)
                            Manager.SetItemEnabled(ItemSaveName, false);
                    };
                }
                return fView;
            }
        }

        protected override void OnCurrentDirectoryChanged() { }

        protected override void RestoreSetting(params string[] args) { }

        protected override void Dispose(bool disposing) {
            Manager.RemoveCommand(ItemViewName);
            Manager.RemoveCommand(ItemEditName);
            Manager.RemoveCommand(ItemSaveName);
            base.Dispose(disposing);
        }

        string GetPath() {
            return Path.Combine(Manager.CurrentDirectory, Manager.GetSelectedFiles().First().Name);
        }

        void ShowView(bool readOnly) {
            string path = GetPath();
            if (!File.Exists(path)) return;
            View.Text = File.ReadAllText(GetPath());
            View.Properties.ReadOnly = readOnly;
            Manager.SetItemEnabled(ItemSaveName, !readOnly);
            Manager.ShowView(View);
        }
    }
}
