using System;
using System.IO;
using System.Linq;
using System.Drawing;
using DevExpress.Utils;
using System.Reflection;
using DevExpress.XtraBars;
using System.Windows.Forms;
using DXFileExplorer.Models;
using System.ComponentModel;
using DXFileExplorer.Actions;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using System.Drawing.Imaging;
using DXFileExplorer.Extensions;
using System.Collections.Generic;
using DevExpress.XtraEditors.ViewInfo;
using DXFileExplorer.Views.Collections;

namespace DXFileExplorer.Views.Controls {
    public partial class FileSystemControl :XtraUserControl {
        const string CurrentFileIndicator = "CurrentFileIndicator";
        const string CurrentSizeIndicator = "CurrentSizeIndicator";
        const string LastModifiedIndicator = "LastModifiedIndicator";
        readonly Dictionary<FileSystemItemType, ImageInfo> FileSystemItemTypeImages =
            new Dictionary<FileSystemItemType, ImageInfo>();
        Bar StateInformationBar;
        BarManager BarManager;
        readonly Dictionary<string, BarItem> StateIndicators = 
            new Dictionary<string, BarItem>();
        bool SelectWhenShift = true;
        Action<string> ReturnEditorResult;

        FileSystemControl() {
            InitializeComponent();
            FileSystemItemTypeImages = new Dictionary<FileSystemItemType, ImageInfo>();
            Assembly asm = Assembly.GetExecutingAssembly();
            FileSystemItemTypeImages.Add(FileSystemItemType.File, GetImage(asm, "file.png"));
            FileSystemItemTypeImages.Add(FileSystemItemType.Directory,
                GetImage(asm, "folderclose.png"));
        }

        public FileSystemControl(FileSystemViewData source, FileSystemViewPosition position,
            BarManager manager) : this() {
            fDataSource = source;
            group.DataBindings.Add("Text", fDataSource, "Path");
            fPosition = position;
            BarManager = manager;
            BarManager.BeginUpdate();
            try {
                BarManager.DockControls.Add(barDockControl);
                StateInformationBar = new Bar(manager, string.Concat("FilesView", Position)) {
                    DockStyle = BarDockStyle.Standalone, CanDockStyle = BarCanDockStyle.Standalone,
                    StandaloneBarDockControl = barDockControl
                };
                StateInformationBar.OptionsBar.AllowQuickCustomization = false;
                StateInformationBar.OptionsBar.DrawDragBorder = false;
                StateInformationBar.OptionsBar.UseWholeRow = true;
                AddStateIndicator(BarItemLinkAlignment.Left, CurrentFileIndicator);
                AddStateIndicator(BarItemLinkAlignment.Right, CurrentSizeIndicator);
                AddStateIndicator(BarItemLinkAlignment.Right, LastModifiedIndicator);
            } finally { BarManager.EndUpdate(); }
            list.DataSource = fDataSource.Source;
            list.DisplayMember = "Name";
            list.ValueMember = "ID";
        }

        public bool IsCurrent { get; set; }

        readonly FileSystemViewPosition fPosition;
        public FileSystemViewPosition Position {
            get { return fPosition; }
        }

        readonly FileSystemViewData fDataSource;
        public FileSystemViewData DataSource {
            get { return fDataSource; }
        }

        public FileSystemItem CurrentItem {
            get { return (FileSystemItem)list.SelectedItem; }
            set { list.SelectedItem = value; }
        }

        readonly List<string> fSelectedFiles = new List<string>();
        public List<FileSystemItem> SelectedFiles {
            get {
                if (fSelectedFiles.Count == 0)
                    return new List<FileSystemItem>() { CurrentItem };
                List<FileSystemItem> result = new List<FileSystemItem>();
                fSelectedFiles.ForEach(i => {
                    FileSystemItem item = (FileSystemItem)list.GetItem(list.FindItem(0, true, a => a.IsFound = (string)a.DisplayItemValue == i));
                    if (item.ItemType != FileSystemItemType.RootDirectory)
                        result.Add(item);
                });
                return result;
            }
        }

        protected override void Dispose(bool disposing) {
            group.DataBindings.Clear();
            list.DataSource = null;
            foreach (KeyValuePair<FileSystemItemType, ImageInfo> kvp in FileSystemItemTypeImages)
                kvp.Value.Image.Dispose();
            FileSystemItemTypeImages.Clear();
            if (StateInformationBar != null) {
                StateInformationBar.ItemLinks.Clear();
                BarManager.Bars.RemoveAt(BarManager.Bars.IndexOf(StateInformationBar));
                StateInformationBar.Dispose();
                StateInformationBar = null;
            }
            foreach (KeyValuePair<string, BarItem> kvp in StateIndicators)
                kvp.Value.Dispose();
            StateIndicators.Clear();
            BarManager = null;
            base.Dispose(disposing);
        }

        private void OnListDrawItem(object sender, ListBoxDrawItemEventArgs e) {
            e.Handled = true;
            BaseListBoxViewInfo.ItemInfo itemInfo = (BaseListBoxViewInfo.ItemInfo)e.GetItemInfo();
            int id = Convert.ToInt32(itemInfo.Item);
            using (AppearanceObject appearance = new AppearanceObject()) {
                appearance.Assign(list.GetViewInfo().PaintAppearance);
                if (IsItemSelected(itemInfo.Text)) {
                    appearance.BackColor = LookAndFeelHelper.GetSystemColor(LookAndFeel, 
                        SystemColors.Highlight);
                    appearance.ForeColor = LookAndFeelHelper.GetSystemColor(LookAndFeel,
                        SystemColors.HighlightText);
                }
                appearance.DrawBackground(e.Cache, e.Bounds);
                FileSystemItem item = DataSource.Source.First(fsi => fsi.ID == id);
                Rectangle imageRectangle = new Rectangle(itemInfo.Bounds.X, itemInfo.Bounds.Y,
                    itemInfo.Bounds.Height, itemInfo.Bounds.Height);
                if (FileSystemItemTypeImages.ContainsKey(item.ItemType)) {
                    ImageInfo imageInfo = FileSystemItemTypeImages[item.ItemType];
                    e.Graphics.DrawImage(imageInfo.Image,
                        new Point[] { 
                        imageRectangle.Location, 
                        new Point(imageRectangle.Right, imageRectangle.Top), 
                        new Point(imageRectangle.Left, imageRectangle.Bottom) },
                            new Rectangle(Point.Empty, imageInfo.Image.Size), GraphicsUnit.Pixel,
                        imageInfo.Attributes);
                }
                const int textIndent = 1;
                e.Cache.Paint.DrawMultiColorString(e.Cache, new Rectangle(imageRectangle.Right +
                    textIndent, itemInfo.Bounds.Y, itemInfo.Bounds.Width - itemInfo.Bounds.Height -
                    textIndent, itemInfo.Bounds.Height), itemInfo.Text, itemInfo.Text.Substring(0, 
                    itemInfo.HightlightCharsCount), appearance, appearance.BackColor,
                    appearance.ForeColor, false);
                if (id == CurrentItem.ID && IsCurrent)
                    e.Cache.Paint.DrawFocusRectangle(e.Graphics, e.Bounds, appearance.ForeColor,
                        appearance.BackColor);
            }
        }

        private static ImageInfo GetImage(Assembly asm, string fileName) {
            using (Stream file = asm.GetManifestResourceStream(string.Concat("DXFileExplorer.Images.",
                fileName))) {
                return new ImageInfo(Image.FromStream(file));
            }
        }

        public void BeginUpdate() {
            list.BeginUpdate();
        }

        public void EndUpdate() {
            list.EndUpdate();
        }

        private void OnListDoubleClick(object sender, EventArgs e) {
            Point pt = MousePosition;
            if (list.GetItemRectangle(list.SelectedIndex).Contains(list.PointToClient(pt)))
                Open();
        }

        void RaiseExecuteAction(ExecuteActionEventArgs args) {
            EventHandler<ExecuteActionEventArgs> handler = Events[fExecuteAction]
                as EventHandler<ExecuteActionEventArgs>;
            if (handler != null)
                handler(this, args);
        }

        private void OnListKeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode)
            {
            	case Keys.Enter:
                    Open();
            		break;
                case Keys.Left:
                    Jump(Math.Max(0,
                        list.SelectedIndex - ((BaseListBoxViewInfo)list.GetViewInfo()).ItemsPerColumn),
                        e.Shift);
                    e.Handled = true;
                    break;
                case Keys.Right:
                    Jump(Math.Min(list.ItemCount - 1,
                        list.SelectedIndex + ((BaseListBoxViewInfo)list.GetViewInfo()).ItemsPerColumn),
                        e.Shift);
                    e.Handled = true;
                    break;
                case Keys.ShiftKey:
                    SelectWhenShift = !IsItemSelected(CurrentItem.Name);
                    break;
                case Keys.Up:
                case Keys.Down:
                    if (e.Shift)
                        DoSelect(CurrentItem.Name);
                    break;
            }
        }

        void OnListSelectedIndexChanged(object sender, EventArgs e) {
            if (list.SelectedIndex < 0) return;
            StateIndicators[CurrentFileIndicator].Caption = list.GetItemText(list.SelectedIndex);
            StateIndicators[CurrentSizeIndicator].Caption = CurrentItem.Size;
            StateIndicators[LastModifiedIndicator].Caption = CurrentItem.LastModifiedDate.GUIToString();
        }

        void AddStateIndicator(BarItemLinkAlignment alignment, string name) {
            BarItem item = new BarStaticItem() {
                Visibility = BarItemVisibility.OnlyInRuntime, Alignment = alignment
            };
            StateIndicators.Add(name, item);
            StateInformationBar.AddItem(item);
        }

        public void UpdateColumnWidth() {
            list.ColumnWidth = list.ClientRectangle.Width / 2;
        }

        bool IsItemSelected(string index) {
            return fSelectedFiles.Contains(index);
        }

        void Open() {
            if (CurrentItem.ItemType == FileSystemItemType.Error) return;
            if (CurrentItem.ItemType == FileSystemItemType.RootDirectory)
                fSelectedFiles.Clear();
            RaiseExecuteAction(new ExecuteOpenEventArgs(CurrentItem.Name, 
                CurrentItem.ItemType == FileSystemItemType.Directory ||
                CurrentItem.ItemType == FileSystemItemType.RootDirectory));
        }

        public void Edit(Action<string> returnEditorResult) {
            Rectangle itemRectangle = list.GetItemRectangle(list.SelectedIndex);
            Editor.Location = group.PointToClient(list.PointToScreen(itemRectangle.Location));
            Editor.Size = itemRectangle.Size;
            Editor.EditValue = list.GetItemText(list.SelectedIndex);
            this.ReturnEditorResult = returnEditorResult;
            Editor.IsModified = true;
            Editor.Visible = true;
            Editor.MaskBox.Focus();
        }

        void OnEditorValidating(object sender, CancelEventArgs e) {
            if (!Editor.Visible) return;
            int index = list.FindString(Editor.Text);
            e.Cancel = index > -1 && index != list.SelectedIndex;
        }

        private void OnEditorKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                ReturnEditorResult(string.Empty);
                HideEditor();
            }
        }

        void HideEditor() {
            Editor.Visible = false;
            ReturnEditorResult = null;
            list.Focus();
        }

        private void OnEditorValidated(object sender, EventArgs e) {
            if (ReturnEditorResult != null) 
                ReturnEditorResult(Editor.Text);
            HideEditor();
        }

        public void ClearSelection() {
            fSelectedFiles.Clear();
        }

        void DoSelect(params string[] items) {
            Action<string> select = SelectWhenShift ? i => {
                if (!IsItemSelected(i))
                    fSelectedFiles.Add(i);
            } : (Action<string>)(i => {
                if (IsItemSelected(i))
                    fSelectedFiles.Remove(i);
            });
            items.ToList().ForEach(i => select(i));
            list.Invalidate();
        }

        void Jump(int nextSelectedIndex, bool select) {
            if (select)
                DoSelect(list.SelectedIndex.GetRange(nextSelectedIndex).Select(i => ((FileSystemItem)list.GetItem(i)).Name).ToArray());
            list.SelectedIndex = nextSelectedIndex;
        }

        static readonly object fExecuteAction = new object();
        public event EventHandler<ExecuteActionEventArgs> ExecuteAction {
            add { Events.AddHandler(fExecuteAction, value); }
            remove { Events.RemoveHandler(fExecuteAction, value); }
        }

        class ImageInfo {
            public ImageInfo(Image image) {
                fImage = image;
                Color pixel = ((Bitmap)image).GetPixel(0, image.Height - 1);
                const int range = 5;
                fAttributes = new ImageAttributes();
                fAttributes.SetColorKey(Color.FromArgb(ValidateColorValue(pixel.R -
                    range), ValidateColorValue(pixel.G - range), ValidateColorValue(pixel.B -
                    range)), Color.FromArgb(ValidateColorValue(pixel.R + range), ValidateColorValue(pixel.G + range),
                    ValidateColorValue(pixel.B + range)));
            }

            static int ValidateColorValue(int value) {
                return Math.Max(0, Math.Min(255, value));
            }

            readonly Image fImage;
            public Image Image {
                get { return fImage; }
            }

            readonly ImageAttributes fAttributes;
            public ImageAttributes Attributes {
                get { return fAttributes; }
            }
        }
    }
}
