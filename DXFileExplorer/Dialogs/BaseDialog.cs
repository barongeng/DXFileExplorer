using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Customization;
using DevExpress.XtraLayout.Utils;

namespace DXFileExplorer.Dialogs {
    public partial class BaseDialog :XtraForm {
        protected BaseDialog() {
            InitializeComponent();
            LayoutContainer.AllowCustomizationMenu = true;
        }

        protected BaseDialog(MessageBoxButtons buttons) : this() {
            SetButtonsCore(buttons);
        }

        public virtual void SetButtons(MessageBoxButtons buttons) {
            SetButtonsCore(buttons);
        }

        private void SetButtonsCore(MessageBoxButtons buttons) {
            LayoutContainer.BeginUpdate();
            try {
                switch (buttons) {
                    case MessageBoxButtons.AbortRetryIgnore:
                        lciCancel.Visibility = LayoutVisibility.Never;
                        lciNo.Visibility = LayoutVisibility.Never;
                        lciOk.Visibility = LayoutVisibility.Never;
                        lciYes.Visibility = LayoutVisibility.Never;
                        lciAbort.Visibility = LayoutVisibility.Always;
                        lciRetry.Visibility = LayoutVisibility.Always;
                        lciIgnore.Visibility = LayoutVisibility.Always;
                        break;
                    case MessageBoxButtons.OK:
                        lciAbort.Visibility = LayoutVisibility.Never;
                        lciCancel.Visibility = LayoutVisibility.Never;
                        lciIgnore.Visibility = LayoutVisibility.Never;
                        lciNo.Visibility = LayoutVisibility.Never;
                        lciRetry.Visibility = LayoutVisibility.Never;
                        lciYes.Visibility = LayoutVisibility.Never;
                        lciOk.Visibility = LayoutVisibility.Always;
                        break;
                    case MessageBoxButtons.OKCancel:
                        lciAbort.Visibility = LayoutVisibility.Never;
                        lciIgnore.Visibility = LayoutVisibility.Never;
                        lciNo.Visibility = LayoutVisibility.Never;
                        lciRetry.Visibility = LayoutVisibility.Never;
                        lciYes.Visibility = LayoutVisibility.Never;
                        lciOk.Visibility = LayoutVisibility.Always;
                        lciCancel.Visibility = LayoutVisibility.Always;
                        break;
                    case MessageBoxButtons.RetryCancel:
                        lciAbort.Visibility = LayoutVisibility.Never;
                        lciIgnore.Visibility = LayoutVisibility.Never;
                        lciNo.Visibility = LayoutVisibility.Never;
                        lciOk.Visibility = LayoutVisibility.Never;
                        lciYes.Visibility = LayoutVisibility.Never;
                        lciRetry.Visibility = LayoutVisibility.Always;
                        lciCancel.Visibility = LayoutVisibility.Always;
                        break;
                    case MessageBoxButtons.YesNo:
                        lciAbort.Visibility = LayoutVisibility.Never;
                        lciCancel.Visibility = LayoutVisibility.Never;
                        lciIgnore.Visibility = LayoutVisibility.Never;
                        lciOk.Visibility = LayoutVisibility.Never;
                        lciRetry.Visibility = LayoutVisibility.Never;
                        lciYes.Visibility = LayoutVisibility.Always;
                        lciNo.Visibility = LayoutVisibility.Always;
                        break;
                    case MessageBoxButtons.YesNoCancel:
                        lciAbort.Visibility = LayoutVisibility.Never;
                        lciIgnore.Visibility = LayoutVisibility.Never;
                        lciOk.Visibility = LayoutVisibility.Never;
                        lciRetry.Visibility = LayoutVisibility.Never;
                        lciYes.Visibility = LayoutVisibility.Always;
                        lciNo.Visibility = LayoutVisibility.Always;
                        lciCancel.Visibility = LayoutVisibility.Always;
                        break;
                }
            } finally { LayoutContainer.EndUpdate(); }
            LayoutContainer.BestFit();
            MinimumSize = LayoutContainer.PreferredSize;
        }

        protected int CommandPanelHeight {
            get { return LayoutContainer.Height; }
        }

        protected void SetOkEnabled(bool enabled) {
            sbOk.Enabled = enabled;
        }
    }
}
