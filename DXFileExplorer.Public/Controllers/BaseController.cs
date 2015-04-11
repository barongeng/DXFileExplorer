using System;
using System.Collections.Generic;
using System.Resources;
using DXFileExplorer.Public;

namespace DXFileExplorer.Controllers {
    public abstract class BaseController :IDisposable {
        protected const char SettingOuterSeparator = ';';
        protected const char SettingInnerSeparator = ',';

        public BaseController(IControllerManager manager) {
            fManager = manager;
            Manager.CurrentDirectoryChanged += (s, e) => OnCurrentDirectoryChanged();
            string settings = Manager.ReadApplicationSetting(SettingsName);
            if (!string.IsNullOrEmpty(settings))
                foreach (string setting in settings.Split(SettingOuterSeparator))
                    RestoreSetting(setting.Split(SettingInnerSeparator));
        }

        ~BaseController() {
            Dispose(false);
        }

        IControllerManager fManager;
        protected IControllerManager Manager {
            get { return fManager; }
        }

        public static string EditPageText {
            get { return Resources.EditPageText; }
        }

        protected abstract string SettingsName { get; }
        protected abstract void OnCurrentDirectoryChanged();
        protected abstract void RestoreSetting(params string[] args);

        protected virtual void Dispose(bool disposing) {
            if (disposing)
                fManager = null;
        }

        protected void WriteApplicationSetting(string value) {
            Manager.WriteApplicationSetting(SettingsName, value);
        }

        #region IDisposable
        void IDisposable.Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
