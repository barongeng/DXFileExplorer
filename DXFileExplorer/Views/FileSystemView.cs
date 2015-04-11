using System;
using System.Linq;
using DevExpress.XtraBars;
using System.Windows.Forms;
using DXFileExplorer.Models;
using DXFileExplorer.Actions;
using DevExpress.XtraEditors;
using System.Collections.Generic;
using DXFileExplorer.Views.Controls;
using DXFileExplorer.Views.Collections;
using DevExpress.Xpo.DB;
using System.Resources;
using System.ComponentModel;
using DXFileExplorer.Controllers;

namespace DXFileExplorer.Views {
    public partial class FileSystemView :BaseView {
        readonly FileSystemViewDataSource Source;
        readonly List<FileSystemControl> ViewControls = new List<FileSystemControl>();

        FileSystemView() {
            InitializeComponent();
        }

        public FileSystemView(FileSystemViewDataSource source, BarManager manager) 
            : this() {
            this.Source = source;
            AddViewControl(splitContainer.Panel1, Source.DataA, FileSystemViewPosition.A,
                manager);
            AddViewControl(splitContainer.Panel2, Source.DataB, FileSystemViewPosition.B,
                manager);
        }

        FileSystemViewPosition fCurrentView;
        public FileSystemViewPosition CurrentView {
            get { return fCurrentView; }
        }

        public FileSystemItem CurrentItem {
            get { return ActiveListControl.CurrentItem; }
        }

        FileSystemControl ActiveListControl {
            get { return ViewControls.First(c => c.IsCurrent); }
        }

        protected override void Dispose(bool disposing) {
            ViewControls.Clear();
            base.Dispose(disposing);
        }

        public void BeginUpdate() {
            foreach (FileSystemControl ctrl in ViewControls)
                ctrl.BeginUpdate();
        }

        public void EndUpdate() {
            foreach (FileSystemControl ctrl in ViewControls)
                ctrl.EndUpdate();
        }

        void RaiseExecuteAction(ExecuteActionEventArgs args) {
            EventHandler<ExecuteActionEventArgs> handler = Events[fExecuteAction]
                as EventHandler<ExecuteActionEventArgs>;
            if (handler != null)
                handler(this, args);
        }

        void RaiseCurrentViewChanged() {
            EventHandler handler = Events[fCurrentViewChanged] as EventHandler;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        void AddViewControl(Control container, FileSystemViewData source, FileSystemViewPosition position,
            BarManager manager) {
            FileSystemControl ctrl = new FileSystemControl(source, position, manager) { 
                Parent = container, Dock = DockStyle.Fill 
            };
            ctrl.ExecuteAction += (s, e) => RaiseExecuteAction(e);
            ctrl.Enter += (s, e) => {
                if (fCurrentView == ctrl.Position) return;
                fCurrentView = ctrl.Position;
                foreach (FileSystemControl fsc in ViewControls)
                    fsc.IsCurrent = fsc == s;
                RaiseCurrentViewChanged();
            };
            ViewControls.Add(ctrl);
        }

        public void ActivateCurrentView() {
            foreach (FileSystemControl fsc in ViewControls)
                if (fsc.IsCurrent) {
                    fsc.Focus();
                    break;
                }
        }

        public IEnumerable<FileSystemItem> GetSelectedFiles() {
            foreach (FileSystemControl fsc in ViewControls)
                if (fsc.IsCurrent)
                    return fsc.SelectedFiles;
            return new FileSystemItem[0];
        }

        void OnFormShown(object sender, EventArgs e) {
            splitContainer.SplitterPosition = Width / 2;
            foreach (FileSystemControl ctrl in ViewControls)
                ctrl.UpdateColumnWidth();
        }

        private void OnSplitContainerSplitterPositionChanged(object sender, EventArgs e) {
            foreach (FileSystemControl ctrl in ViewControls)
                ctrl.UpdateColumnWidth();
        }

        public void Edit(Action<string> returnEditorResult) {
            ActiveListControl.Edit(returnEditorResult);
        }

        public void SetItemSelected(FileSystemItem item) {
            ActiveListControl.CurrentItem = item;
        }

        public void ClearSelection() {
            ActiveListControl.ClearSelection();
        }

        static readonly object fExecuteAction = new object();
        public event EventHandler<ExecuteActionEventArgs> ExecuteAction {
            add { Events.AddHandler(fExecuteAction, value); }
            remove { Events.RemoveHandler(fExecuteAction, value); }
        }

        static readonly object fCurrentViewChanged = new object();
        public event EventHandler CurrentViewChanged {
            add { Events.AddHandler(fCurrentViewChanged, value); }
            remove { Events.RemoveHandler(fCurrentViewChanged, value); }
        }
    }

    [Flags]
    public enum FileSystemViewPosition { A = 1, B = 2}
}