namespace DXFileExplorer.Dialogs {
    public partial class ErrorDialog {
        void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorDialog));
            this.lcMessage = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // lcMessage
            // 
            this.lcMessage.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lcMessage.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            resources.ApplyResources(this.lcMessage, "lcMessage");
            this.lcMessage.Name = "lcMessage";
            // 
            // ErrorDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lcMessage);
            this.Name = "ErrorDialog";
            this.Controls.SetChildIndex(this.lcMessage, 0);
            this.ResumeLayout(false);

        }

        private DevExpress.XtraEditors.LabelControl lcMessage;
    }
}
