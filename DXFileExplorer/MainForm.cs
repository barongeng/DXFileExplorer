using System;
using System.IO;
using DevExpress.XtraBars;
using DXFileExplorer.Views;
using System.ComponentModel;
using DevExpress.XtraNavBar;
using DevExpress.LookAndFeel;
using DXFileExplorer.Controllers;
using System.Collections.Generic;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Configuration;
using DevExpress.XtraBars.Docking;
using DXFileExplorer.Properties;
using DXFileExplorer.Actions;
using DXFileExplorer.Utils;
using DevExpress.XtraEditors;
using DXFileExplorer.Models;
using DXFileExplorer.Public.Utils;

namespace DXFileExplorer {
    public partial class MainForm :RibbonForm, IControllerManager {
        readonly DXFileExplorerSettings Settings;
        readonly FileSystemController Controller;
        readonly List<IDisposable> Controllers = new List<IDisposable>();
        bool CanRaiseCurrentDirectoryChanged = true;

        public MainForm() {
            InitializeComponent();
            Settings = new DXFileExplorerSettings();
            Controller = new FileSystemController(this);
            CurrentDirectoryA = Settings.CurrentDirectoryA;
            CurrentDirectoryB = Settings.CurrentDirectoryB;
            SkinHelper.InitSkinGallery(rgbiSkins);
            Controller.Show();
            SkinName = Settings.SkinName;
        }

        public string SkinName {
            get { return UserLookAndFeel.Default.ActiveSkinName; }
            set { UserLookAndFeel.Default.SkinName = value; }
        }

        public string CurrentDirectoryA { get; set; }

        public string CurrentDirectoryB { get; set; }

        public NavBarControl NavPane {
            get { return navPane; }
        }

        public BarManager BarManager {
            get { return ribbon.Manager; }
        }

        protected override void OnClosing(CancelEventArgs e) {
            Settings.SkinName = SkinName;
            Settings.CurrentDirectoryA = CurrentDirectoryA;
            Settings.CurrentDirectoryB = CurrentDirectoryB;
            using (MemoryStream stream = new MemoryStream()) {
                DockManager.SaveLayoutToStream(stream);
                Settings.DockManagerLayout = Convert.ToBase64String(stream.GetBuffer());
            }
            foreach (IDisposable controller in Controllers)
                controller.Dispose();
            Settings.Save();
            CanRaiseCurrentDirectoryChanged = false;
            ((IDisposable)Controller).Dispose();
            base.OnClosing(e);
        }

        public string GetCurrentDirectory(FileSystemViewPosition pos) {
            return pos == FileSystemViewPosition.A ? CurrentDirectoryA : CurrentDirectoryB;
        }

        public void SetCurrentDirectory(FileSystemViewPosition pos, string path) {
            try {
                path = new DirectoryInfo(path).FullName;
                if (pos == FileSystemViewPosition.A)
                    CurrentDirectoryA = path;
                else CurrentDirectoryB = path;
            } catch (ArgumentException e) {
                XtraMessageBox.Show(this, e.Message, Resources.DXFileExplorer, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateText(string text) {
            this.Text = !string.IsNullOrEmpty(text) ? 
                string.Concat("{", text, "} - ", "DX File Explorer") : "DX File Explorer";
            if (CanRaiseCurrentDirectoryChanged)
                RaiseCurrentDirectoryChanged();
        }

        public string GetDestinationDirectory(FileSystemViewPosition pos) {
            return pos == FileSystemViewPosition.A ? CurrentDirectoryB : CurrentDirectoryA;
        }

        private void OnMerge(object sender, RibbonMergeEventArgs e) {
            SelectFirstPage();
        }

        void LoadControllers(Assembly assembly, bool third) {
            Type baseController = typeof(BaseController);
            int fileSystemController = typeof(FileSystemController).GetHashCode();
            int baseControllerHash = baseController.GetHashCode();
            assembly.GetTypes().ToList().ForEach(t => {
                int hash = t.GetHashCode();
                if (baseController.IsAssignableFrom(t) && (third || (fileSystemController != hash && baseControllerHash != hash)))
                    Controllers.Add((IDisposable)Activator.CreateInstance(t, this));
            });
        }

        BarItem CreateCommand(BarSubItem parent, CommandInfo info) {
            bool menu = info.CommandName.Contains(';');
            string name = menu ? info.CommandName.Substring(0, info.CommandName.IndexOf(';')) : info.CommandName;
            BarItem result = null;
            if (parent == null)
                result = Ribbon.Items.Cast<BarItem>().FirstOrDefault(i => i.Name == name);
            else {
                BarItemLink link = parent.ItemLinks.Cast<BarItemLink>().FirstOrDefault(l => l.Item.Name == name);
                if (link != null)
                    result = link.Item;
            }
            if (result == null)
                if (menu) {
                    string commandText = info.CommandText.Substring(0, info.CommandText.IndexOf(';'));
                    BarSubItem subItem = new BarSubItem(Ribbon.Manager, commandText);
                    result = subItem;
                    info.CommandName = info.CommandName.Replace(name, string.Empty).TrimStart(';');
                    info.CommandText = info.CommandText.Replace(commandText, string.Empty).TrimStart(';');
                    subItem.ItemLinks.Add(CreateCommand(subItem, info));
                } else {
                    BarButtonItem button = new BarButtonItem(Ribbon.Manager, info.CommandText,
                        -1, info.SecondKey == Keys.None ? new BarShortcut(info.Key) : new BarShortcut(info.Key, info.SecondKey));
                    button.ItemClick += (s, e) => info.Callback(e.Item.Tag);
                    switch (info.CommandStyle) {
                        case CommandStyle.Check: button.ButtonStyle = BarButtonStyle.Check; break;
                        case CommandStyle.Default: button.ButtonStyle = BarButtonStyle.Default; break;
                        case CommandStyle.DropDown: button.ButtonStyle = BarButtonStyle.DropDown; break;
                    }
                    button.Down = info.IsDown;
                    Ribbon.Items.Add(button);
                    if (info.DownChangedCallback != null)
                        button.DownChanged += (s, e) => info.DownChangedCallback(info.Tag);
                    result = button;
                }
            result.Name = name;
            result.Tag = info.Tag;
            result.Hint = info.Tooltip;
            return result;
        }

        void HideItem(BarItem item) {
            BarSubItem menu = item as BarSubItem;
            item.Visibility = BarItemVisibility.OnlyInRuntime;
            if (menu != null)
                foreach (BarItemLink link in menu.ItemLinks)
                    HideItem(link.Item);
        }

        void OnClosedPanel(object sender, DockPanelEventArgs e) {
            AddPaneNavigationItem(e.Panel);
        }

        private void AddPaneNavigationItem(DockPanel pane) {
            const string name = "Panels";
            NavBarGroup group = NavPane.Groups[name];
            if (group == null)
                group = NavPane.Groups.Add(new NavBarGroup(Resources.NavPaneGroupCaptionWindows) { Name = name });
            NavBarItem item = group.AddItem().Item;
            item.Caption = pane.Text;
            item.Name = string.Join("_", name, pane.Name);
            item.Tag = pane.Name;
            item.LinkClicked += (s, ea) => {
                NavBarItem i = (NavBarItem)s;
                DockManager.Panels[i.Tag.ToString()].Visibility = DockVisibility.Visible;
                i.Dispose();
                if (group.ItemLinks.Count == 0)
                    group.Dispose();
            };
        }

        void OnFormLoad(object sender, EventArgs e) {
            LoadControllers(Assembly.GetExecutingAssembly(), false);
            new FileInfo(Application.ExecutablePath).Directory.EnumerateFiles("*Plugin.dll").ToList().ForEach(i => LoadControllers(Assembly.LoadFile(i.FullName), true));
            string layout = Settings.DockManagerLayout;
            if (string.IsNullOrEmpty(layout)) return;
            using (Stream stream = new MemoryStream(Convert.FromBase64String(layout))) {
                DockManager.RestoreLayoutFromStream(stream);
            }
            foreach (DockPanel panel in DockManager.HiddenPanels)
                AddPaneNavigationItem(panel);
            Controller.ActivateView();
            bbiCloseActiveView.ItemShortcut = new BarShortcut(Keys.Escape);
            SelectFirstPage();
        }

        void OnFormActivated(object sender, EventArgs e) {
            if (ActiveMdiChild is FileSystemView) 
                new ExecuteRefreshEventArgs().Accept(Controller);
        }

        void OnCloseActiveViewItemClick(object sender, ItemClickEventArgs e) {
            if (!(ActiveMdiChild is FileSystemView)) {
                ActiveMdiChild.Close();
                ActiveMdiChild.WindowState = FormWindowState.Maximized;
            }
        }

        void SelectFirstPage() {
            if (Ribbon.MergedPages.Count == 0) {
                if (Ribbon.Pages.Count > 0)
                    Ribbon.SelectedPage = Ribbon.Pages[0];
            } else Ribbon.SelectedPage = Ribbon.MergedPages[0];
        }

        #region IControllerManager
        bool IControllerManager.AddCommand(CreateCommandArgs args) {
            if (Ribbon.Items.Cast<BarItem>().Any(i => i.Name == args.Info.CommandName)) return false;
            RibbonPage page;
            RibbonPageGroup group = null;
            if (!(string.IsNullOrEmpty(args.PageText) || string.IsNullOrEmpty(args.GroupText))) {
                page = Ribbon.Pages.Cast<RibbonPage>().FirstOrDefault(p => p.Text == args.PageText);
                if (page == null)
                    page = Ribbon.MergedPages.Cast<RibbonPage>().FirstOrDefault(p => p.Text == args.PageText);
                if (page == null) {
                    page = new RibbonPage(args.PageText);
                    if (args.PageText == BaseController.EditPageText)
                        Ribbon.Pages.Insert(0, page);
                    else Ribbon.Pages.Add(page);
                }
                group = page.Groups.Cast<RibbonPageGroup>().FirstOrDefault(g => g.Text == args.GroupText);
                if (group == null) {
                    group = new RibbonPageGroup(args.GroupText);
                    page.Groups.Add(group);
                    group.ItemLinks.CollectionChanged += (s, e) => {
                        if (group.ItemLinks.Count == 0)
                            if (group.Page.Groups.Count == 1)
                                group.Page.Dispose();
                            else group.Dispose();
                    };
                }
            }
            BarItem command = CreateCommand(null, args.Info);
            if (group != null)
                group.ItemLinks.Add(command);
            else HideItem(command);
            return true;
        }

        void IControllerManager.RemoveCommand(string commandName) {
            BarItem command = Ribbon.Items.Cast<BarItem>().FirstOrDefault(i => i.Caption == commandName);
            if (command != null)
                command.Dispose();
        }

        string IControllerManager.ReadApplicationSetting(string name) {
            if (string.IsNullOrEmpty(name)) return string.Empty;
            return Settings.GetControllerSetting(name);
        }

        void IControllerManager.WriteApplicationSetting(string name, string value) {
            if (string.IsNullOrEmpty(name)) return;
            Settings.SetControllerSetting(name, value);
        }

        string IControllerManager.CurrentDirectory {
            get { return Controller.GetCurrentDirectory(); }
            set { Controller.ChangeDirectory(value); }
        }

        void IControllerManager.AddPane(Control content, PaneDockingStyle dock, Guid id) {
            DockPanel panel = DockManager.Panels[content.Name];
            if (panel == null)
                panel = DockManager.AddPanel((DockingStyle)Enum.Parse(typeof(DockingStyle), dock.ToString()));
            panel.Text = content.Text;
            foreach (Control control in panel.ControlContainer.Controls)
                control.Dispose();
            content.Parent = panel.ControlContainer;
            panel.ID = id;
        }

        void IControllerManager.RemovePane(string name) {
            DockManager.RemovePanel(DockManager.Panels[name]);
        }

        static readonly object fCurrentDirectoryChanged = new object();
        event EventHandler IControllerManager.CurrentDirectoryChanged {
            add { Events.AddHandler(fCurrentDirectoryChanged, value); }
            remove { Events.RemoveHandler(fCurrentDirectoryChanged, value); }
        }

        void RaiseCurrentDirectoryChanged() {
            if (!CanRaiseCurrentDirectoryChanged) return;
            EventHandler handler = (EventHandler)Events[fCurrentDirectoryChanged];
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        void IControllerManager.ActivateFileSystemView() {
            Controller.ActivateView();
        }

        void IControllerManager.ExecuteFile(string file, string args) {
            new ExecuteOpenEventArgs(file, args).Accept(Controller);
        }

        void IControllerManager.AddNavItem(CreateNavItemArgs args) {
            if (NavPane.Items.Cast<NavBarItem>().Any(i => i.Name == args.ItemName))
                return;
            NavBarGroup group = NavPane.Groups.Cast<NavBarGroup>().FirstOrDefault(g => g.Name == args.GroupName);
            if (group == null)
                group = NavPane.Groups.Add(new NavBarGroup(args.GroupText) { Name = args.GroupName });
            NavBarItem item = args.Index == -1 ? group.AddItem().Item : group.InsertItem(args.Index).Item;
            item.Name = args.ItemName;
            item.Caption = args.ItemText;
            item.LinkClicked += (s, e) => args.Callback((string)((NavElement)s).Tag);
            item.Tag = args.Location;
        }

        void IControllerManager.RemoveNavItem(string name) {
            NavBarItem item = NavPane.Items.Cast<NavBarItem>().FirstOrDefault(i => i.Name == name);
            if (item == null) return;
            for (int i = item.Links.Count - 1; i >= 0; i--) {
                NavBarItemLink link = item.Links[i];
                if (link.Group.ItemLinks.Count == 1)
                    link.Group.Dispose();
            }
            item.Dispose();
        }

        void IControllerManager.ShowDialog(Form dialog) {
            dialog.ShowDialog(this);
        }

        void IControllerManager.ShowError(string message, MessageBoxButtons buttons, MessageBoxIcon icon) {
            XtraMessageBox.Show(this, message, Resources.DXFileExplorer, buttons, icon);
        }

        IEnumerable<FileSystemItem> IControllerManager.GetSelectedFiles() {
            return Controller.GetSelectedFiles();
        }

        void IControllerManager.ShowView(Control view) {
            Form form = view as Form;
            if (form == null) {
                form = new BaseView() { MdiParent = this };
                view.Parent = form;
                form.FormClosing += (s, e) => view.Parent = null;
            }
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        void IControllerManager.SetItemEnabled(string name, bool enabled) {
            BarItem item = Ribbon.Items.Cast<BarItem>().FirstOrDefault(i => i.Name == name);
            if (item != null)
                item.Enabled = enabled;
        }

        void IControllerManager.ChangeItemCaption(string name, string caption) {
            BarItem item = Ribbon.Items.Cast<BarItem>().FirstOrDefault(i => i.Name == name);
            if (item != null)
                item.Caption = caption;
        }
        #endregion
    }
}
