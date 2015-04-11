using System.Windows.Forms;
using DXFileExplorer.Controllers;
using DXFileExplorer.UranusPack.Properties;
using DXFileExplorer.Utils;
using System.Linq;
using AxWMPLib;
using System.Collections.Generic;
using System.IO;
using WMPLib;
using System;
using System.Drawing;

namespace DXFileExplorer.UranusPack.Controllers {
    public class PlayMusicController :BaseController {
        const string PlayCommandName = "Uranus_Player_Play";
        IList<string> PlayList;
        AxWindowsMediaPlayer Player = new AxWindowsMediaPlayer();

        public PlayMusicController(IControllerManager manager) : base(manager) {
            Manager.AddCommand(new Utils.CreateCommandArgs() {
                PageText = "Uranus", GroupText = Resources.Player_GroupText,
                Info = new CommandInfo() {
                    CommandName = PlayCommandName, CommandText = Resources.Player_PlayCommandText,
                    Key = Keys.F12,
                    Callback = o => {
                        PlayList = Manager.GetSelectedFiles().Where(i => i.Name.ToLowerInvariant(
                            ).EndsWith(".mp3")).Select(i => Path.Combine(Manager.CurrentDirectory, i.Name)).ToList();
                        if (Player.playState != WMPPlayState.wmppsPlaying)
                            PlayNext();
                    }
                }
            });
            Player.PlayStateChange += (s, e) => {
                if ((WMPPlayState)e.newState == WMPPlayState.wmppsStopped)
                    PlayNext();
            };
            Player.Parent = (Control)Manager;
            Player.Visible = false;
        }

        protected override string SettingsName {
            get { return "Uranus_Player"; }
        }

        protected override void OnCurrentDirectoryChanged() { }

        protected override void RestoreSetting(params string[] args) { }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                Manager.RemoveCommand(PlayCommandName);
                Player.Dispose();
                Player = null;
            }
            base.Dispose(disposing);
        }

        void PlayNext() {
            if (PlayList.Count == 0) return;
            Player.URL = PlayList[0];
            Player.Ctlcontrols.play();
            PlayList.RemoveAt(0);
        }
    }
}
