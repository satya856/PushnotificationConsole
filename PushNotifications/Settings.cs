#region

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

#endregion

namespace PushNotifications
{
    public class Settings
    {
        private static Settings instance;

        private string _ApnsCertificateFile;

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    var baseDir = AppDomain.CurrentDomain.BaseDirectory;

                    string settingsFile;

                    baseDir = baseDir.Replace("bin\\Debug\\", "");
                    settingsFile = Path.Combine(baseDir, "App_Data\\settings.json");

                    instance = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsFile));
                }
                return instance;
            }
        }

        [JsonProperty("apns_cert_file")]
        public string ApnsCertificateFile
        {
            get
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                baseDir = baseDir.Replace("bin\\Debug\\", "");
                return baseDir + _ApnsCertificateFile;
            }
            set { _ApnsCertificateFile = value; }
        }

        [JsonProperty("apns_cert_pwd")]
        public string ApnsCertificatePassword { get; set; }

        [JsonProperty("apns_device_tokens")]
        public List<string> ApnsDeviceTokens { get; set; }
    }
}