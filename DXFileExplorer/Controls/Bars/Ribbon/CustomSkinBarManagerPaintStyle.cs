using DevExpress.XtraBars.Styles;
using DevExpress.XtraBars.ViewInfo;
using DevExpress.XtraBars.Painters;
using DevExpress.XtraBars.InternalItems;

namespace DXFileExplorer.Controls.Bars.Ribbon {
    public class CustomSkinBarManagerPaintStyle : SkinBarManagerPaintStyle {
        public CustomSkinBarManagerPaintStyle(BarManagerPaintStyleCollection collection)
            : base(collection) { }

        public override string Name {
            get { return "Skin Custom"; }
        }

        protected override void RegisterItemInfo() {
            base.RegisterItemInfo();
            const string name = "RibbonExpandCollapseItem";
            ItemInfoCollection.Remove(ItemInfoCollection[name]);
            ItemInfoCollection.Add(new BarInternalItemInfo(name, typeof(RibbonExpandCollapseItem),
                typeof(CustomRibbonExpandCollapseItemLink), typeof(RibbonExpandCollapseItemLinkViewInfo),
                new BarButtonOffice2003LinkPainter(this)));
        }
    }
}
