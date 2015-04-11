using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DXFileExplorer.Dialogs {
    public partial class ProgressDialog :XtraForm {
        string Operation;

        ProgressDialog() {
            InitializeComponent();
        }

        public static Action<string, string> ShowProgress(Form owner, string operation) {
            ProgressDialog dialog = new ProgressDialog();
            dialog.Width = Math.Min(owner.Width, owner.Height) / 3 * 2;
            dialog.Height = dialog.Width / 10;
            dialog.Location = owner.PointToScreen(
                new Point((owner.Width - dialog.Width) / 2, 
                    (owner.Height - dialog.Height) / 2));
            dialog.SetOperation(operation);
            owner.Enabled = false;
            dialog.Show(GetDialogOwner(owner));
            return (o, n) => {
                if (string.IsNullOrEmpty(n)) {
                    dialog.Close();
                    dialog.Dispose();
                    owner.Enabled = true;
                } else dialog.UpdateProgressMessage(o, n);
            };
        }

        static Control GetDialogOwner(Control owner) {
            return owner.Parent == null ? owner : GetDialogOwner(owner.Parent);
        }

        void SetOperation(string operation) {
            this.Operation = operation;
            marqueeProgressBarControl1.Text = this.Operation;
        }

        void UpdateProgressMessage(string obj, string name) {
            if (marqueeProgressBarControl1.InvokeRequired)
                marqueeProgressBarControl1.Invoke(new Action<string, string>(UpdateProgressMessage),
                    obj, name);
            else
                marqueeProgressBarControl1.Text = string.Concat(Operation, "\r\n", 
                    Operation.TrimEnd('e'), "ing the ", obj, ":\r\n", name);
        }
    }
}
