using DevExpress.XtraBars.Ribbon.ViewInfo;

namespace DXFileExplorer.Controls.Bars.Ribbon {
    public class CustomRibbonViewInfo :RibbonViewInfo {
        public CustomRibbonViewInfo(CustomRibbonControl ribbon) : base(ribbon) { }

        internal bool IsEmptyTabHeaderInternal {
            get { return IsEmptyTabHeader; }
        }

        protected override RibbonPageHeaderViewInfo CreateHeaderInfo() {
            return new CustomRibbonPageHeaderViewInfo(this);
        }
    }
}
