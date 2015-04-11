using DevExpress.Skins;
using DevExpress.XtraEditors.Drawing;

namespace DXFileExplorer.Views.Controls.CommandLine {
    public class CommandLinePainter :BaseControlPainter {
        protected override void DrawContent(ControlGraphicsInfoArgs info) {
            SkinElementPainter.Default.DrawObject(new SkinElementInfo(CommonSkins.GetSkin(info.ViewInfo.LookAndFeel)[CommonSkins.SkinGroupPanel]) {
                Bounds = info.ViewInfo.ContentRect, Cache = info.Cache
            });
            info.ViewInfo.PaintAppearance.DrawString(info.Cache, info.ViewInfo.DisplayText, info.ViewInfo.ClientRect);
        }
    }
}
