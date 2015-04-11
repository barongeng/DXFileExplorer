using System.ComponentModel;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Ribbon.ViewInfo;

namespace DXFileExplorer.Controls.Bars.Ribbon {
    public class CustomRibbonControl :RibbonControl {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new CustomRibbonBarManager Manager {
            get { return (CustomRibbonBarManager)base.Manager; }
        }

        internal bool GetShowExpandCollapseButtonInternal() {
            return GetShowExpandCollapseButton();
        }

        protected override RibbonBarManager CreateBarManager() {
            return new CustomRibbonBarManager(this);
        }

        protected override RibbonViewInfo CreateViewInfo() {
            return new CustomRibbonViewInfo(this);
        }
    }
}
