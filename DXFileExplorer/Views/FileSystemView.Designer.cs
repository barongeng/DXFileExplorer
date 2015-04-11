namespace DXFileExplorer.Views {
    public partial class FileSystemView {
        void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileSystemView));
            this.splitContainer = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            resources.ApplyResources(this.splitContainer, "splitContainer");
            this.splitContainer.Name = "splitContainer";
            resources.ApplyResources(this.splitContainer.Panel1, "splitContainer.Panel1");
            resources.ApplyResources(this.splitContainer.Panel2, "splitContainer.Panel2");
            this.splitContainer.SplitterPosition = 157;
            this.splitContainer.SplitterPositionChanged += new System.EventHandler(this.OnSplitContainerSplitterPositionChanged);
            // 
            // FileSystemView
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer);
            this.Name = "FileSystemView";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.OnFormShown);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private DevExpress.XtraEditors.SplitContainerControl splitContainer;
    }
}
