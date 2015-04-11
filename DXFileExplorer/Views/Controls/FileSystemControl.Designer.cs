namespace DXFileExplorer.Views.Controls {
    public partial class FileSystemControl {
        void InitializeComponent() {
            this.group = new DevExpress.XtraEditors.GroupControl();
            this.Editor = new DevExpress.XtraEditors.TextEdit();
            this.list = new DevExpress.XtraEditors.ListBoxControl();
            this.barDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.group)).BeginInit();
            this.group.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Editor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.list)).BeginInit();
            this.SuspendLayout();
            // 
            // group
            // 
            this.group.Controls.Add(this.Editor);
            this.group.Controls.Add(this.list);
            this.group.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group.Location = new System.Drawing.Point(0, 0);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(210, 127);
            this.group.TabIndex = 0;
            this.group.Text = "groupControl1";
            // 
            // Editor
            // 
            this.Editor.Location = new System.Drawing.Point(6, 26);
            this.Editor.Name = "Editor";
            this.Editor.Properties.ValidateOnEnterKey = true;
            this.Editor.Size = new System.Drawing.Size(100, 20);
            this.Editor.TabIndex = 1;
            this.Editor.Visible = false;
            this.Editor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEditorKeyDown);
            this.Editor.Validating += new System.ComponentModel.CancelEventHandler(this.OnEditorValidating);
            this.Editor.Validated += new System.EventHandler(this.OnEditorValidated);
            // 
            // list
            // 
            this.list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list.IncrementalSearch = true;
            this.list.Location = new System.Drawing.Point(2, 22);
            this.list.MultiColumn = true;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(206, 103);
            this.list.TabIndex = 0;
            this.list.SelectedIndexChanged += new System.EventHandler(this.OnListSelectedIndexChanged);
            this.list.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.OnListDrawItem);
            this.list.DoubleClick += new System.EventHandler(this.OnListDoubleClick);
            this.list.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnListKeyDown);
            // 
            // barDockControl
            // 
            this.barDockControl.CausesValidation = false;
            this.barDockControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControl.Location = new System.Drawing.Point(0, 127);
            this.barDockControl.Name = "barDockControl";
            this.barDockControl.Size = new System.Drawing.Size(210, 23);
            this.barDockControl.Text = "standaloneBarDockControl1";
            // 
            // FileSystemControl
            // 
            this.Controls.Add(this.group);
            this.Controls.Add(this.barDockControl);
            this.Name = "FileSystemControl";
            this.Size = new System.Drawing.Size(210, 150);
            ((System.ComponentModel.ISupportInitialize)(this.group)).EndInit();
            this.group.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Editor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.list)).EndInit();
            this.ResumeLayout(false);

        }

        private DevExpress.XtraEditors.GroupControl group;
        private DevExpress.XtraEditors.ListBoxControl list;
        private DevExpress.XtraBars.StandaloneBarDockControl barDockControl;
        private DevExpress.XtraEditors.TextEdit Editor;
    }
}
