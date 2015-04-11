using System;
using DevExpress.XtraBars;
using DevExpress.XtraBars.ViewInfo;
using DevExpress.XtraBars.InternalItems;

namespace DXFileExplorer.Controls.Bars.Ribbon {
    public class CustomRibbonExpandCollapseItemLink :RibbonExpandCollapseItemLink {
        public CustomRibbonExpandCollapseItemLink(BarItemLinkReadOnlyCollection links, 
            BarItem item, object linkedObject) : base(links, item, linkedObject) { }

        internal BarLinkViewInfo LinkViewInfoInternal {
            get { return LinkViewInfo; }
        }
    }
}
