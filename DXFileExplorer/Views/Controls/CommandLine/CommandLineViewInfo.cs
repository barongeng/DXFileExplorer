using System;
using System.Drawing;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Utils.Text;
using DevExpress.XtraEditors.ViewInfo;

namespace DXFileExplorer.Views.Controls.CommandLine {
    public class CommandLineViewInfo :BaseControlViewInfo {
        public CommandLineViewInfo(CommandLineView owner) : base(owner) { }

        public override string DisplayText {
            get {
                return string.Concat(string.IsNullOrEmpty(OwnerControl.DisplayText) ? null :
                    string.Concat(OwnerControl.DisplayText, Environment.NewLine),
                    OwnerControl.CurrentDirectory, CommandLineView.Pointer, OwnerControl.Command);
            }
        }

        protected virtual new CommandLineView OwnerControl {
            get { return (CommandLineView)base.OwnerControl; }
        }

        protected override AppearanceObject CreatePaintAppearance() {
            AppearanceObject result = base.CreatePaintAppearance();
            result.TextOptions.VAlignment = VertAlignment.Top;
            result.TextOptions.WordWrap = WordWrap.Wrap;
            return result;
        }

        protected override ObjectInfoArgs GetBorderArgs(Rectangle bounds) {
            int y = bounds.Bottom - TextSize.Height - TextUtils.GetStringHeight(GInfo.Graphics, "Q", PaintAppearance.Font, bounds.Width);
            if (y < bounds.Y) {
                bounds.Y = y;
                bounds.Height -= y;
            }
            return base.GetBorderArgs(bounds);
        }

        public override void CalcViewInfo(Graphics g) {
            CalcTextSize(g, true);
            base.CalcViewInfo(g);
        }
    }
}
