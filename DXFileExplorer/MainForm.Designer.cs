namespace DXFileExplorer
{
	public partial class MainForm {
        void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ribbon = new DXFileExplorer.Controls.Bars.Ribbon.CustomRibbonControl();
            this.rgbiSkins = new DevExpress.XtraBars.RibbonGalleryBarItem();
            this.bbiCloseActiveView = new DevExpress.XtraBars.BarButtonItem();
            this.rpView = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rpgAppearance = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.navPane = new DevExpress.XtraNavBar.NavBarControl();
            this.DockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navPane)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DockManager)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            resources.ApplyResources(this.ribbon, "ribbon");
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.ExpandCollapseItem.Name = "";
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.rgbiSkins,
            this.bbiCloseActiveView});
            this.ribbon.MaxItemId = 3;
            this.ribbon.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Never;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpView});
            this.ribbon.Merge += new DevExpress.XtraBars.Ribbon.RibbonMergeEventHandler(this.OnMerge);
            // 
            // rgbiSkins
            // 
            resources.ApplyResources(this.rgbiSkins, "rgbiSkins");
            this.rgbiSkins.Id = 1;
            this.rgbiSkins.Name = "rgbiSkins";
            // 
            // bbiCloseActiveView
            // 
            resources.ApplyResources(this.bbiCloseActiveView, "bbiCloseActiveView");
            this.bbiCloseActiveView.Id = 2;
            this.bbiCloseActiveView.Name = "bbiCloseActiveView";
            this.bbiCloseActiveView.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.OnCloseActiveViewItemClick);
            // 
            // rpView
            // 
            this.rpView.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rpgAppearance});
            this.rpView.Name = "rpView";
            resources.ApplyResources(this.rpView, "rpView");
            // 
            // rpgAppearance
            // 
            this.rpgAppearance.ItemLinks.Add(this.rgbiSkins);
            this.rpgAppearance.Name = "rpgAppearance";
            resources.ApplyResources(this.rpgAppearance, "rpgAppearance");
            // 
            // navPane
            // 
            this.navPane.ActiveGroup = null;
            resources.ApplyResources(this.navPane, "navPane");
            this.navPane.Name = "navPane";
            this.navPane.OptionsNavPane.ExpandedWidth = ((int)(resources.GetObject("resource.ExpandedWidth")));
            this.navPane.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            // 
            // DockManager
            // 
            this.DockManager.Form = this;
            this.DockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DXFileExplorer.Controls.Bars.Ribbon.CustomRibbonControl",
            "DevExpress.XtraNavBar.NavBarControl"});
            this.DockManager.ClosedPanel += new DevExpress.XtraBars.Docking.DockPanelEventHandler(this.OnClosedPanel);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.navPane);
            this.Controls.Add(this.ribbon);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Ribbon = this.ribbon;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.OnFormActivated);
            this.Load += new System.EventHandler(this.OnFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navPane)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DockManager)).EndInit();
            this.ResumeLayout(false);

        }

        private DXFileExplorer.Controls.Bars.Ribbon.CustomRibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpView;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgAppearance;
        private DevExpress.XtraBars.RibbonGalleryBarItem rgbiSkins;
        private DevExpress.XtraNavBar.NavBarControl navPane;
        private System.ComponentModel.IContainer components;
        private DevExpress.XtraBars.Docking.DockManager DockManager;
        private DevExpress.XtraBars.BarButtonItem bbiCloseActiveView;
	}
}
