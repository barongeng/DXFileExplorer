using System.Reflection;
using DevExpress.XtraBars;
using DevExpress.Utils.Drawing;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Ribbon.ViewInfo;

namespace DXFileExplorer.Controls.Bars.Ribbon {
    public class CustomRibbonPageHeaderViewInfo :RibbonPageHeaderViewInfo {
        RibbonItemViewInfo ExpandCollapseItemInfo;

        public CustomRibbonPageHeaderViewInfo(CustomRibbonViewInfo viewInfo) : base(viewInfo) { }

        protected override void CreateMDIItemsViewInfo() { }

        public new CustomRibbonControl Ribbon {
            get { return (CustomRibbonControl)base.Ribbon; }
        }

        public new CustomRibbonViewInfo ViewInfo {
            get { return (CustomRibbonViewInfo)base.ViewInfo; }
        }

        CustomRibbonExpandCollapseItemLink ExpandCollapseItemLink {
            get {
                return (CustomRibbonExpandCollapseItemLink)typeof(RibbonControl).GetProperty("ExpandCollapseItemLink",
                    BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Ribbon,
                    null);
            }
        }

        protected override void UpdateSystemLinkGlyph(BarItemLink link, ObjectState state) {
            if (link == ExpandCollapseItemLink && Ribbon.GetShowExpandCollapseButtonInternal() &&
                ExpandCollapseItemInfo != null)
                ExpandCollapseItemInfo.ExtraGlyph = Ribbon.Minimized ? GetExpandButtonGlyphImage(state) :
                    GetCollapseButtonGlyphImage(state);
        }

        protected override void CreateExpandCollapseItemViewInfo() {
            bool isExpandCollapseItemInPageHeaderItemLinks = false;
            foreach (BarItemLink link in Ribbon.PageHeaderItemLinks)
                if (link.Item == Ribbon.ExpandCollapseItem) {
                    isExpandCollapseItemInPageHeaderItemLinks = true;
                    break;
                }
            if (!Ribbon.GetShowExpandCollapseButtonInternal() || isExpandCollapseItemInPageHeaderItemLinks ||
                ViewInfo.IsEmptyTabHeaderInternal)
                return;
            ExpandCollapseItemInfo = Ribbon.Manager.CreateItemViewInfoInternal(ViewInfo,
                ExpandCollapseItemLink);
            ExpandCollapseItemLink.LinkViewInfoInternal.UpdateLinkState();
            PageHeaderItems.Add(ExpandCollapseItemInfo);
        }
    }
}
