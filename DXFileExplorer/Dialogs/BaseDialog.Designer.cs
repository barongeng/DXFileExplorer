namespace DXFileExplorer.Dialogs {
    public partial class BaseDialog {
        void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseDialog));
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.LayoutContainer = new DevExpress.XtraLayout.LayoutControl();
            this.sbNo = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.sbIgnore = new DevExpress.XtraEditors.SimpleButton();
            this.sbRetry = new DevExpress.XtraEditors.SimpleButton();
            this.sbAbort = new DevExpress.XtraEditors.SimpleButton();
            this.sbOk = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciCancel = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciOk = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciAbort = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciRetry = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciIgnore = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciYes = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.LayoutContainer)).BeginInit();
            this.LayoutContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAbort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRetry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciIgnore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciYes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.simpleButton2, "simpleButton2");
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.StyleController = this.LayoutContainer;
            // 
            // LayoutContainer
            // 
            this.LayoutContainer.AllowCustomizationMenu = false;
            this.LayoutContainer.Controls.Add(this.sbNo);
            this.LayoutContainer.Controls.Add(this.simpleButton5);
            this.LayoutContainer.Controls.Add(this.sbIgnore);
            this.LayoutContainer.Controls.Add(this.sbRetry);
            this.LayoutContainer.Controls.Add(this.sbAbort);
            this.LayoutContainer.Controls.Add(this.simpleButton2);
            this.LayoutContainer.Controls.Add(this.sbOk);
            resources.ApplyResources(this.LayoutContainer, "LayoutContainer");
            this.LayoutContainer.Name = "LayoutContainer";
            this.LayoutContainer.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(638, 64, 450, 350);
            this.LayoutContainer.Root = this.layoutControlGroup1;
            // 
            // sbNo
            // 
            this.sbNo.DialogResult = System.Windows.Forms.DialogResult.No;
            resources.ApplyResources(this.sbNo, "sbNo");
            this.sbNo.Name = "sbNo";
            this.sbNo.StyleController = this.LayoutContainer;
            // 
            // simpleButton5
            // 
            this.simpleButton5.DialogResult = System.Windows.Forms.DialogResult.Yes;
            resources.ApplyResources(this.simpleButton5, "simpleButton5");
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.StyleController = this.LayoutContainer;
            // 
            // sbIgnore
            // 
            this.sbIgnore.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            resources.ApplyResources(this.sbIgnore, "sbIgnore");
            this.sbIgnore.Name = "sbIgnore";
            this.sbIgnore.StyleController = this.LayoutContainer;
            // 
            // sbRetry
            // 
            this.sbRetry.DialogResult = System.Windows.Forms.DialogResult.Retry;
            resources.ApplyResources(this.sbRetry, "sbRetry");
            this.sbRetry.Name = "sbRetry";
            this.sbRetry.StyleController = this.LayoutContainer;
            // 
            // sbAbort
            // 
            this.sbAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
            resources.ApplyResources(this.sbAbort, "sbAbort");
            this.sbAbort.Name = "sbAbort";
            this.sbAbort.StyleController = this.LayoutContainer;
            // 
            // sbOk
            // 
            this.sbOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.sbOk, "sbOk");
            this.sbOk.Name = "sbOk";
            this.sbOk.StyleController = this.LayoutContainer;
            // 
            // layoutControlGroup1
            // 
            resources.ApplyResources(this.layoutControlGroup1, "layoutControlGroup1");
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciCancel,
            this.lciOk,
            this.lciAbort,
            this.lciRetry,
            this.lciIgnore,
            this.lciYes,
            this.lciNo,
            this.emptySpaceItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(346, 46);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lciCancel
            // 
            this.lciCancel.Control = this.simpleButton2;
            resources.ApplyResources(this.lciCancel, "lciCancel");
            this.lciCancel.Location = new System.Drawing.Point(46, 0);
            this.lciCancel.MaxSize = new System.Drawing.Size(54, 26);
            this.lciCancel.MinSize = new System.Drawing.Size(54, 26);
            this.lciCancel.Name = "lciCancel";
            this.lciCancel.Size = new System.Drawing.Size(54, 26);
            this.lciCancel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciCancel.TextSize = new System.Drawing.Size(0, 0);
            this.lciCancel.TextToControlDistance = 0;
            this.lciCancel.TextVisible = false;
            // 
            // lciOk
            // 
            this.lciOk.Control = this.sbOk;
            resources.ApplyResources(this.lciOk, "lciOk");
            this.lciOk.Location = new System.Drawing.Point(10, 0);
            this.lciOk.MaxSize = new System.Drawing.Size(54, 26);
            this.lciOk.MinSize = new System.Drawing.Size(36, 26);
            this.lciOk.Name = "lciOk";
            this.lciOk.Size = new System.Drawing.Size(36, 26);
            this.lciOk.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciOk.TextSize = new System.Drawing.Size(0, 0);
            this.lciOk.TextToControlDistance = 0;
            this.lciOk.TextVisible = false;
            // 
            // lciAbort
            // 
            this.lciAbort.Control = this.sbAbort;
            resources.ApplyResources(this.lciAbort, "lciAbort");
            this.lciAbort.Location = new System.Drawing.Point(100, 0);
            this.lciAbort.MaxSize = new System.Drawing.Size(54, 26);
            this.lciAbort.MinSize = new System.Drawing.Size(49, 26);
            this.lciAbort.Name = "lciAbort";
            this.lciAbort.Size = new System.Drawing.Size(49, 26);
            this.lciAbort.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciAbort.TextSize = new System.Drawing.Size(0, 0);
            this.lciAbort.TextToControlDistance = 0;
            this.lciAbort.TextVisible = false;
            // 
            // lciRetry
            // 
            this.lciRetry.Control = this.sbRetry;
            resources.ApplyResources(this.lciRetry, "lciRetry");
            this.lciRetry.Location = new System.Drawing.Point(149, 0);
            this.lciRetry.MaxSize = new System.Drawing.Size(54, 26);
            this.lciRetry.MinSize = new System.Drawing.Size(49, 26);
            this.lciRetry.Name = "lciRetry";
            this.lciRetry.Size = new System.Drawing.Size(49, 26);
            this.lciRetry.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciRetry.TextSize = new System.Drawing.Size(0, 0);
            this.lciRetry.TextToControlDistance = 0;
            this.lciRetry.TextVisible = false;
            // 
            // lciIgnore
            // 
            this.lciIgnore.Control = this.sbIgnore;
            resources.ApplyResources(this.lciIgnore, "lciIgnore");
            this.lciIgnore.Location = new System.Drawing.Point(198, 0);
            this.lciIgnore.MaxSize = new System.Drawing.Size(54, 26);
            this.lciIgnore.MinSize = new System.Drawing.Size(54, 26);
            this.lciIgnore.Name = "lciIgnore";
            this.lciIgnore.Size = new System.Drawing.Size(54, 26);
            this.lciIgnore.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciIgnore.TextSize = new System.Drawing.Size(0, 0);
            this.lciIgnore.TextToControlDistance = 0;
            this.lciIgnore.TextVisible = false;
            // 
            // lciYes
            // 
            this.lciYes.Control = this.simpleButton5;
            resources.ApplyResources(this.lciYes, "lciYes");
            this.lciYes.Location = new System.Drawing.Point(252, 0);
            this.lciYes.MaxSize = new System.Drawing.Size(54, 26);
            this.lciYes.MinSize = new System.Drawing.Size(39, 26);
            this.lciYes.Name = "lciYes";
            this.lciYes.Size = new System.Drawing.Size(39, 26);
            this.lciYes.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciYes.TextSize = new System.Drawing.Size(0, 0);
            this.lciYes.TextToControlDistance = 0;
            this.lciYes.TextVisible = false;
            // 
            // lciNo
            // 
            this.lciNo.Control = this.sbNo;
            resources.ApplyResources(this.lciNo, "lciNo");
            this.lciNo.Location = new System.Drawing.Point(291, 0);
            this.lciNo.MaxSize = new System.Drawing.Size(54, 26);
            this.lciNo.MinSize = new System.Drawing.Size(35, 26);
            this.lciNo.Name = "lciNo";
            this.lciNo.Size = new System.Drawing.Size(35, 26);
            this.lciNo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciNo.TextSize = new System.Drawing.Size(0, 0);
            this.lciNo.TextToControlDistance = 0;
            this.lciNo.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            resources.ApplyResources(this.emptySpaceItem1, "emptySpaceItem1");
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(10, 26);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // BaseDialog
            // 
            this.AcceptButton = this.sbOk;
            this.CancelButton = this.simpleButton2;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.LayoutContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BaseDialog";
            ((System.ComponentModel.ISupportInitialize)(this.LayoutContainer)).EndInit();
            this.LayoutContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAbort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRetry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciIgnore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciYes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);

        }
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton sbOk;
        private DevExpress.XtraLayout.LayoutControl LayoutContainer;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem lciCancel;
        private DevExpress.XtraLayout.LayoutControlItem lciOk;
        private DevExpress.XtraEditors.SimpleButton sbAbort;
        private DevExpress.XtraLayout.LayoutControlItem lciAbort;
        private DevExpress.XtraEditors.SimpleButton sbRetry;
        private DevExpress.XtraLayout.LayoutControlItem lciRetry;
        private DevExpress.XtraEditors.SimpleButton sbIgnore;
        private DevExpress.XtraLayout.LayoutControlItem lciIgnore;
        private DevExpress.XtraEditors.SimpleButton sbNo;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraLayout.LayoutControlItem lciYes;
        private DevExpress.XtraLayout.LayoutControlItem lciNo;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}
