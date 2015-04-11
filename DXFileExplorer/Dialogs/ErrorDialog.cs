using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Data.Helpers;
using DevExpress.Utils.Text;

namespace DXFileExplorer.Dialogs {
    public partial class ErrorDialog :BaseDialog {
        const MessageBoxButtons DefaultButtons = MessageBoxButtons.AbortRetryIgnore;

        ErrorDialog() : base(DefaultButtons) {
            InitializeComponent();    
        }

        public ErrorDialog(string caption, string message) : this() {
            Text = caption;
            lcMessage.Text = message;
            using (Graphics g = CreateGraphics()) {
                Size textSize = TextUtils.GetStringSize(g, message, lcMessage.Appearance.Font);
                const int textMargin = 20;
                textSize.Height += CommandPanelHeight + textMargin;
                textSize.Width += textMargin;
                ClientSize = textSize;
            }
        }

        public override void SetButtons(MessageBoxButtons buttons) {
            if (buttons == DefaultButtons) return;
            base.SetButtons(buttons);
        }
    }
}
