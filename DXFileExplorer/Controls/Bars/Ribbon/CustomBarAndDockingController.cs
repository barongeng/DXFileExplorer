using System;
using DevExpress.XtraBars;
using System.ComponentModel;

namespace DXFileExplorer.Controls.Bars.Ribbon {
    public class CustomBarAndDockingController :BarAndDockingController {
        public CustomBarAndDockingController(IContainer container) : base(container) { }
        public CustomBarAndDockingController() :base() { }

        protected override void RegisterPaintStyles() {
            PaintStyles.Add(new CustomSkinBarManagerPaintStyle(PaintStyles));
        }
    }
}
