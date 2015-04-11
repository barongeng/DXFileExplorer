using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace DXFileExplorer {
    public class DXFileExplorerSettings :ApplicationSettingsBase {
        const string DefaultCurrentDirectory = @"C:\";
        Dictionary<string, string> fControllersSettings;

        [UserScopedSetting, DefaultSettingValue("DevExpress Style")]
        public string SkinName {
            get { return this["SkinName"].ToString(); }
            set { this["SkinName"] = value; }
        }

        [UserScopedSetting, DefaultSettingValue(DefaultCurrentDirectory)]
        public string CurrentDirectoryA {
            get { 
                string result = (string)this["CurrentDirectoryA"]; 
                return string.IsNullOrEmpty(result) ? DefaultCurrentDirectory : result; 
            }
            set { this["CurrentDirectoryA"] = value; }
        }

        [UserScopedSetting, DefaultSettingValue(DefaultCurrentDirectory)]
        public string CurrentDirectoryB {
            get { 
                string result = (string)this["CurrentDirectoryB"];
                return string.IsNullOrEmpty(result) ? DefaultCurrentDirectory : result; 
            }
            set { this["CurrentDirectoryB"] = value; }
        }

        [UserScopedSetting]
        public string ControllersSettings {
            get { return (string)this["ControllersSettings"]; }
            set { this["ControllersSettings"] = value; }
        }

        [UserScopedSetting]
        public string DockManagerLayout {
            get { return (string)this["DockManagerLayout"]; }
            set { this["DockManagerLayout"] = value; }
        }

        public string GetControllerSetting(string key) {
            if (fControllersSettings == null) {
                fControllersSettings = new Dictionary<string, string>();
                string data = ControllersSettings;
                if (!string.IsNullOrEmpty(data)) {
                    string[] settings = data.Split('|');
                    for (int i = 0; i < settings.Length; i += 2)
                        fControllersSettings.Add(settings[i], settings[i + 1]);
                }
            }
            return fControllersSettings.ContainsKey(key) ? fControllersSettings[key] : string.Empty;
        }

        public void SetControllerSetting(string key, string value) {
            fControllersSettings[key] = value;
            List<string> settings = new List<string>();
            foreach (KeyValuePair<string, string> kvp in fControllersSettings)
                settings.Add(string.Concat(kvp.Key, "|", kvp.Value));
            ControllersSettings = string.Join("|", settings);
        }
    }
}
