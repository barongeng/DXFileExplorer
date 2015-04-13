using System;
using System.IO;
using System.Linq;
using System.Dynamic;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using DXFileExplorer.Views;
using System.ComponentModel;
using DXFileExplorer.Models;
using DevExpress.XtraNavBar;
using DXFileExplorer.Dialogs;
using DXFileExplorer.Actions;
using DevExpress.XtraEditors;
using DXFileExplorer.Extensions;
using System.Collections.Generic;
using DXFileExplorer.Views.Collections;
using DXFileExplorer.Properties;
using DevExpress.Xpo.DB;
using System.Linq.Expressions;
using System.Reflection;
using DevExpress.Data.Helpers;
using DXFileExplorer.Utils;

namespace DXFileExplorer.Controllers {
    public class FileSystemController :BaseController, IActionVisitor {
        const string DrivesGroupName = "nbgDrives";
        const string ItemCopyName = "Default_FileSystem_ItemCopy";
        const string ItemMoveName = "Default_FileSystem_ItemMove";
        const string ItemRenameName = "Default_FileSystem_ItemRename";
        const string ItemMakeFolderName = "Default_FileSystem_ItemMakeFolder";
        const string ItemDeleteName = "Default_FileSystem_ItemDelete";
        const string ItemSortByNameName = "Default_FileSystem_ItemSortByName";
        const string ItemSortByCreationDateName = "Default_FileSystem_ItemSortByCreationDate";
        const char SortSignAscending = '^';
        const char SortSignDescending = 'v';
        const string SortingMemberName = "Name";
        const string SortingMemberCreationDate = "LastModifiedDate";

        FileSystemView View;
        FileSystemViewDataSource Source;
        bool Disposed;
        bool SkipSortItemClick;
        char CurrentSortDirection = SortSignAscending;
        string SortingMember = "Name";

        public FileSystemController(MainForm container) : base(container) {
            Manager.AddCommand(new CreateCommandArgs() {
                PageText = EditPageText, GroupText = Resources.Default_FileSystem_GroupText,
                Info = new CommandInfo() {
                    CommandName = ItemCopyName, CommandText = Resources.Default_FileSystem_ItemCopyText,
                    Key = Keys.F5,
                    Callback = o => new ExecuteCopyEventArgs().Accept(this)
                }
            });
            Manager.AddCommand(new CreateCommandArgs() {
                PageText = EditPageText, GroupText = Resources.Default_FileSystem_GroupText,
                Info = new CommandInfo() {
                    CommandName = ItemMoveName, CommandText = Resources.Default_FileSystem_ItemMoveText,
                    Key = Keys.F6,
                    Callback = o => new ExecuteMoveEventArgs().Accept(this)
                }
            });
            Manager.AddCommand(new CreateCommandArgs() {
                PageText = EditPageText, GroupText = Resources.Default_FileSystem_GroupText,
                Info = new CommandInfo() {
                    CommandName = ItemRenameName, CommandText = Resources.Default_FileSystem_ItemRenameText,
                    Key = Keys.Control | Keys.R,
                    Callback = o => new ExecuteRenameEventArgs().Accept(this)
                }
            });
            Manager.AddCommand(new CreateCommandArgs() {
                PageText = EditPageText, GroupText = Resources.Default_FileSystem_GroupText,
                Info = new CommandInfo() {
                    CommandName = ItemMakeFolderName, CommandText = Resources.Default_FileSystem_ItemMakeFolderText,
                    Key = Keys.F7,
                    Callback = o => new ExecuteMakeDirectoryEventArgs().Accept(this)
                }
            });
            Manager.AddCommand(new CreateCommandArgs() {
                PageText = EditPageText, GroupText = Resources.Default_FileSystem_GroupText,
                Info = new CommandInfo() {
                    CommandName = ItemDeleteName, CommandText = Resources.Default_FileSystem_ItemDeleteText,
                    Key = Keys.Control | Keys.Delete,
                    Callback = o => new ExecuteDeleteEventArgs().Accept(this)
                }
            });
            Manager.AddCommand(new CreateCommandArgs() {
                PageText = EditPageText, GroupText = Resources.Default_FileSystem_Sort_Group,
                Info = new CommandInfo() {
                    CommandName = ItemSortByNameName, CommandText = Resources.Default_FileSystem_Sort_ByName,
                    Key = Keys.Control | Keys.F3, Tag = SortingMemberName,
                    Callback = o => OnSortItemClick(), 
                    DownChangedCallback = o => OnSortItemCheckedChanged((string)o)
                }
            });
            Manager.AddCommand(new CreateCommandArgs() {
                PageText = EditPageText, GroupText = Resources.Default_FileSystem_Sort_Group,
                Info = new CommandInfo() {
                    CommandName = ItemSortByCreationDateName, CommandText = Resources.Default_FileSystem_Sort_ByCreationDate,
                    Key = Keys.Control | Keys.F5, Tag = SortingMemberCreationDate,
                    Callback = o => OnSortItemClick(),
                    DownChangedCallback = o => OnSortItemCheckedChanged((string)o)
                }
            });
        }

        protected override string SettingsName {
            get { return string.Empty; }
        }

        public SortingDirection SortingDirection {
            get { return CurrentSortDirection == SortSignAscending ? SortingDirection.Ascending : SortingDirection.Descending; }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (!Disposed)
                    Hide();
                Disposed = true;
                if (View != null) {
                    View.Dispose();
                    View = null;
                }
            }
            base.Dispose(disposing);
        }

        MainForm Container {
            get { return (MainForm)Manager; }
        }

        public void Show() {
            if (View == null) {
                UpdateSource(FileSystemViewPosition.A);
                UpdateSource(FileSystemViewPosition.B);
                View = new FileSystemView(Source, Container.BarManager) {
                    MdiParent = Container
                };
                View.ExecuteAction += OnViewExecuteAction;
                View.CurrentViewChanged += OnViewCurrentViewChanged;
            }
            View.Show();
            NavBarGroup group = (NavBarGroup)Container.NavPane.Groups.Insert(0, new NavBarGroup(Resources.NavPaneGroupCaptionDrives));
            group.Name = DrivesGroupName;
            foreach (DriveInfo drive in DriveInfo.GetDrives()) {
                NavBarItem item = group.AddItem().Item;
                item.Caption = drive.Name;
                item.Tag = drive.RootDirectory.FullName;
                item.LinkClicked += (s, e) => {
                    ChangeDirectory(e.Link.Item.Tag.ToString());
                    View.ActivateCurrentView();
                };
            }
            Container.UpdateText(Container.GetCurrentDirectory(View.CurrentView));
            Container.NavPane.ActiveGroup = group;
        }

        public void Hide() {
            View.ExecuteAction -= OnViewExecuteAction;
            View.CurrentViewChanged -= OnViewCurrentViewChanged;
            Container.NavPane.Groups.Remove(Container.NavPane.Groups[DrivesGroupName]);
            Container.UpdateText(string.Empty);
            View.Hide();
            Manager.RemoveCommand(ItemCopyName);
            Manager.RemoveCommand(ItemMoveName);
            Manager.RemoveCommand(ItemRenameName);
            Manager.RemoveCommand(ItemMakeFolderName);
            Manager.RemoveCommand(ItemDeleteName);
            Manager.RemoveCommand(ItemSortByNameName);
            Manager.RemoveCommand(ItemSortByCreationDateName);
        }

        public void ChangeDirectory(string path) {
            string currentDirectory = Container.GetCurrentDirectory(View.CurrentView);
            Container.SetCurrentDirectory(View.CurrentView, path);
            UpdateSource(View.CurrentView);
            Container.UpdateText(Container.GetCurrentDirectory(View.CurrentView));
            View.SetItemSelected(GetViewData(View.CurrentView).Source.FirstOrDefault(i => i.Name == Path.GetFileName(currentDirectory)));
        }

        FileSystemViewData GetViewData(FileSystemViewPosition pos/*cannot use the current during initialization*/) {
            return pos == FileSystemViewPosition.A ? Source.DataA : Source.DataB;
        }

        void UpdateSource(FileSystemViewPosition pos/*cannot use the current during initialization*/) {
            if (Source == null)
                Source = new FileSystemViewDataSource();
            string path = Container.GetCurrentDirectory(pos);
            UpdateSource(GetViewData(pos), path);
        }

        void UpdateSource(FileSystemViewData source, string path) {
            if (View != null)
                View.BeginUpdate();
            try {
                source.Source.Clear();
                IEnumerable<FileSystemItem> data = DataLayer.GetFileSystemItemsFromDirectory(path);
                IQueryable q = data.AsQueryable();
                ParameterExpression p = Expression.Parameter(typeof(FileSystemItem));
                LambdaExpression l = Expression.Lambda(Expression.MakeMemberAccess(p, typeof(FileSystemItem).GetProperty(SortingMember)), p);
                data = q.Provider.CreateQuery<FileSystemItem>(Expression.Call(typeof(Queryable),
                    SortingDirection == SortingDirection.Ascending ? "OrderBy" : "OrderByDescending",
                    new Type[] { q.ElementType, l.Body.Type },
                    q.Expression, Expression.Quote(l)));
                data.OrderBy(i => i.ItemType).ToList().ForEach(i => source.Source.Add(i));
                source.Path = path;
            } finally {
                if (View != null)
                    View.EndUpdate();
            }
        }

        void OnViewExecuteAction(object sender, ExecuteActionEventArgs e) {
            e.Accept(this);
        }

        void ExecuteFile(string path, string args) {
            try {
                Process.Start(path, args);
            } catch (SystemException ex) {
                XtraMessageBox.Show(ex.Message, "DXFileExplorer", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void OnViewCurrentViewChanged(object sender, EventArgs e) {
            Container.UpdateText(Container.GetCurrentDirectory(View.CurrentView));
        }

        bool IsDirectoryExist(string name) {
            Func<FileSystemItem, bool> condition = i => i.ItemType == FileSystemItemType.Directory && 
                i.Name == name;
            return View.CurrentView == FileSystemViewPosition.A ? Source.DataA.Source.Any(condition) : 
                Source.DataB.Source.Any(condition);
        }

        void UpdateNotActiveViewIfRequired() {
            if (Container.CurrentDirectoryA == Container.CurrentDirectoryB)
                UpdateSource(View.CurrentView == FileSystemViewPosition.A ? FileSystemViewPosition.B :
                    FileSystemViewPosition.A);
        }

        public string GetCurrentDirectory() {
            return Container.GetCurrentDirectory(View.CurrentView);
        }

        protected override void OnCurrentDirectoryChanged() {
            if (Directory.Exists(Manager.CurrentDirectory))
                Environment.CurrentDirectory = Manager.CurrentDirectory;
        }

        public void ActivateView() {
            View.ActivateCurrentView();
        }

        protected override void RestoreSetting(params string[] args) { }

        public IEnumerable<FileSystemItem> GetSelectedFiles() {
            return View.GetSelectedFiles();
        }

        void UpdateSortItemCaption() {
            CurrentSortDirection = CurrentSortDirection == SortSignAscending ? SortSignDescending : SortSignAscending;
            ComponentResourceManager resourceManager = new ComponentResourceManager(GetType());
            switch (SortingMember) {
                case SortingMemberName: 
                    Manager.ChangeItemCaption(ItemSortByNameName, string.Concat(Resources.Default_FileSystem_Sort_ByName, " ", CurrentSortDirection)); 
                    break;
                case SortingMemberCreationDate: 
                    Manager.ChangeItemCaption(ItemSortByCreationDateName, string.Concat(Resources.Default_FileSystem_Sort_ByCreationDate, " ", CurrentSortDirection)); 
                    break;
            }
            new ExecuteRefreshEventArgs().Accept(this);
        }

        void OnSortItemClick() {
            if (SkipSortItemClick)
                SkipSortItemClick = false;
            else UpdateSortItemCaption();
        }

        void OnSortItemCheckedChanged(string sortingMember) {
            SkipSortItemClick = true;
            CurrentSortDirection = SortSignDescending;
            SortingMember = sortingMember;
            UpdateSortItemCaption();
        }

        class FileOperationExecuter {
            Action<FileSystemInfo, string, Action<string, string>> Operation;
            ExecuteFileOperationArguments Arguments;
            FileSystemController Controller;

            public FileOperationExecuter(Action<FileSystemInfo, string, Action<string, string>> operation, ExecuteFileOperationArguments args, FileSystemController controller) {
                this.Operation = operation;
                Arguments = args;
                this.Controller = controller;
            }

            public void ExecuteFileOperation(string operationName, FileSystemViewPosition toRefresh) {
                Arguments.UpdateProgressCallback = ProgressDialog.ShowProgress(Controller.View, operationName);
                ThreadPool.QueueUserWorkItem(new WaitCallback(s => {
                    ExecuteFileOperationArguments a = (ExecuteFileOperationArguments)s;
                    foreach (FileSystemItem fsi in a.SelectedFiles)
                        if (ExecuteFileOperation(a, fsi)) break;
                    Controller.View.Invoke(new MethodInvoker(() => {
                        a.UpdateProgressCallback(string.Empty, string.Empty);
                        if ((toRefresh & FileSystemViewPosition.A) == FileSystemViewPosition.A)
                            Controller.UpdateSource(FileSystemViewPosition.A);
                        if ((toRefresh & FileSystemViewPosition.B) == FileSystemViewPosition.B)
                            Controller.UpdateSource(FileSystemViewPosition.B);
                        Controller.View.ClearSelection();
                        Controller.ActivateView();
                    }));
                }), Arguments.Clone());
            }

            private bool ExecuteFileOperation(ExecuteFileOperationArguments a, FileSystemItem fsi) {
                try {
                    string path = Path.Combine(a.Source, fsi.Name);
                    Operation(fsi.ItemType == FileSystemItemType.Directory ? 
                        (FileSystemInfo)new DirectoryInfo(path) : (FileSystemInfo)new FileInfo(path), a.Destination, a.UpdateProgressCallback);
                    return false;
                } catch (SystemException ex) {
                    bool? result = a.Arguments.ProcessDialogResult((DialogResult)Controller.View.Invoke(new Func<SystemException, DialogResult>(m => {
                        using (ErrorDialog dialog = new ErrorDialog(MasterDetailHelper.SplitPascalCaseString(ex.GetType().Name), a.Arguments.GetExceptionMessage(ex.Message))) {
                            dialog.SetButtons(a.Arguments.ErrorDialogButtons);
                            return dialog.ShowDialog(Controller.View);
                        }
                    }), ex));
                    return result == null ? ExecuteFileOperation(a, fsi) : result == true;
                }
            }
        }

        class ExecuteFileOperationArguments {
            public ExecuteFileOperationArguments(ExecuteActionEventArgs arguments) {
                fArguments = arguments;
            }

            public Action<string, string> UpdateProgressCallback { get; set; }

            List<FileSystemItem> fSelectedFiles = new List<FileSystemItem>();
            public List<FileSystemItem> SelectedFiles {
                get { return fSelectedFiles; }
            }

            string fSource;
            public string Source {
                get { return fSource; }
                set { fSource = value; }
            }

            string fDestination;
            public string Destination {
                get { return fDestination; }
                set { fDestination = value; }
            }

            ExecuteActionEventArgs fArguments;
            public ExecuteActionEventArgs Arguments {
                get { return fArguments; }
            }

            public ExecuteFileOperationArguments Clone() {
                ExecuteFileOperationArguments result = new ExecuteFileOperationArguments(Arguments) {
                    fDestination = Destination, fSource = Source, UpdateProgressCallback = UpdateProgressCallback
                };
                result.SelectedFiles.AddRange(SelectedFiles);
                return result;
            }
        }

        ExecuteFileOperationArguments CreateExecuteFileOperationArguments(ExecuteActionEventArgs arguments) {
            ExecuteFileOperationArguments result = new ExecuteFileOperationArguments(arguments);
            result.Source = Container.GetCurrentDirectory(View.CurrentView);
            result.Destination = Container.GetDestinationDirectory(View.CurrentView);
            result.SelectedFiles.AddRange(GetSelectedFiles());
            return result;
        }

        #region IActionVisitor
        void IActionVisitor.Visit(ExecuteOpenEventArgs e) {
            string path = e.NeedCombinePath ? Path.Combine(View.CurrentView == FileSystemViewPosition.A ?
                Container.CurrentDirectoryA : Container.CurrentDirectoryB, e.Path) : e.Path;
            if (e.IsDirectory)
                ChangeDirectory(path);
            else ExecuteFile(path, e.Arguments);
        }

        void IActionVisitor.Visit(ExecuteCopyEventArgs e) {
            new FileOperationExecuter(e.DoOperation, CreateExecuteFileOperationArguments(e), 
                this).ExecuteFileOperation(Resources.Default_FileSystem_OperationName_Copy, 
                View.CurrentView == FileSystemViewPosition.A ? FileSystemViewPosition.B : FileSystemViewPosition.A);
        }

        void IActionVisitor.Visit(ExecuteMoveEventArgs e) {
            new FileOperationExecuter(e.DoOperation, CreateExecuteFileOperationArguments(e), this
                ).ExecuteFileOperation(Resources.Default_FileSystem_OperationName_Move, FileSystemViewPosition.A | FileSystemViewPosition.B);
        }
        
        void IActionVisitor.Visit(ExecuteMakeDirectoryEventArgs e) {
            string name;
            for (int i = 0; IsDirectoryExist(name = string.Concat("NewFolder", i)); i++) { }
            FileSystemItem dir = new FileSystemItem() {
                ItemType = FileSystemItemType.Directory, LastModifiedDate = DateTime.Now,
                Name = name, Size = "Directory"
            };
            Action<BindingList<FileSystemItem>> addNew = l => {
                dir.ID = l.GetNextID();
                l.Add(dir);
            };
            addNew(View.CurrentView == FileSystemViewPosition.A ? Source.DataA.Source :
                Source.DataB.Source);
            View.SetItemSelected(dir);
            View.Edit(n => {
                if (string.IsNullOrEmpty(n)) {
                    Action<BindingList<FileSystemItem>> removeNew = l => l.RemoveAt(l.Count -
                        1);
                    removeNew(View.CurrentView == FileSystemViewPosition.A ? Source.DataA.Source :
                        Source.DataB.Source);
                } else {
                    Directory.CreateDirectory(Path.Combine(Container.GetCurrentDirectory(View.CurrentView),
                        n));
                    UpdateNotActiveViewIfRequired();
                    dir.Name = n;
                }
            });
        }

        void IActionVisitor.Visit(ExecuteRenameEventArgs e) {
            FileSystemItem currentItem = View.CurrentItem;
            if (currentItem.ItemType != FileSystemItemType.Directory &&
                currentItem.ItemType != FileSystemItemType.File)
                return;
            View.Edit(n => {
                if (string.IsNullOrEmpty(n)) return;
                string currentDirectory = Container.GetCurrentDirectory(View.CurrentView);
                string src = Path.Combine(currentDirectory, currentItem.Name);
                string dest = Path.Combine(currentDirectory, n);
                if (src == dest) return;
                if (currentItem.ItemType == FileSystemItemType.Directory)
                    Directory.Move(src, dest);
                else if (currentItem.ItemType == FileSystemItemType.File)
                    File.Move(src, dest);
                currentItem.Name = n;
                UpdateNotActiveViewIfRequired();
            });
        }

        void IActionVisitor.Visit(ExecuteDeleteEventArgs e) {
            IEnumerable<FileSystemItem> selected = GetSelectedFiles();
            FileSystemItem item = selected.Count() == 1 ? selected.First() : null;
            string itemName, type;
            if (item == null) {
                itemName = selected.Count().ToString();
                type = "items";
            } else {
                if (item.ItemType != FileSystemItemType.Directory &&
                    item.ItemType != FileSystemItemType.File)
                    return;
                itemName = item.Name;
                type = item.ItemType.ToString().ToLowerInvariant();
            }
            FileSystemViewPosition position = View.CurrentView;
            if (Container.CurrentDirectoryA == Container.CurrentDirectoryB)
                position |= View.CurrentView == FileSystemViewPosition.A ? FileSystemViewPosition.B :
                    FileSystemViewPosition.A;
            if (XtraMessageBox.Show(View, string.Format(Properties.Resources.SureToDeleteMessage,
                itemName, type), Properties.Resources.DXFileExplorer, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes) {
                new FileOperationExecuter(e.DoOperation, CreateExecuteFileOperationArguments(e), this
                    ).ExecuteFileOperation("Delete", position);
            }
        }

        void IActionVisitor.Visit(ExecuteRefreshEventArgs e) {
            UpdateSource(View.CurrentView);
        }
        #endregion
    }
}
