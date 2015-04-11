using System;
using DevExpress.XtraBars;
using System.ComponentModel;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Ribbon.ViewInfo;

namespace DXFileExplorer.Controls.Bars.Ribbon {
    public class CustomRibbonBarManager :RibbonBarManager {
        BarAndDockingController fController;
        bool IsDisposed;

        public CustomRibbonBarManager(RibbonControl ribbon) : base(ribbon) { }

        internal RibbonItemViewInfo CreateItemViewInfoInternal(BaseRibbonViewInfo viewInfo,
            IRibbonItem item) {
                return CreateItemViewInfo(viewInfo, item);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override BarAndDockingController Controller {
            get { return GetController(); }
            set { }
        }

        public override BarAndDockingController GetController() {
            if (IsDisposed) throw new ObjectDisposedException(GetType().Name);
            if (fController == null)
                fController = new CustomBarAndDockingController();
            return fController;
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if (disposing) {
                IsDisposed = true;
                if (fController != null) {
                    fController.Dispose();
                    fController = null;
                }
            }
        }
    }
}
