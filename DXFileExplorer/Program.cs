using System;
using DevExpress.Skins;
using System.Windows.Forms;
using DevExpress.UserSkins;

namespace DXFileExplorer {
    static class Program {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BonusSkins.Register();
            OfficeSkins.Register();
            SkinManager.EnableFormSkins();
            Application.Run(new MainForm());
        }
    }
}
