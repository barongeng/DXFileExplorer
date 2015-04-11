using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DXFileExplorer.Views {
    public partial class BaseView :XtraForm {
        public BaseView() {
            InitializeComponent();
        }

        protected override void OnStyleChanged(EventArgs e) {
            base.OnStyleChanged(e);
            if (IsHandleCreated)
                BeginInvoke(new Action(() => WindowState = FormWindowState.Maximized));
        }
    }
}
