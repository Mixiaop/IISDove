using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using U.Utilities.Web;

namespace IISDove.CorpWeixin
{
    [Serializable]
    public class Settings
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public string CorpId { get; set; }

        /// <summary>
        /// 授权应用Id
        /// </summary>
        public string AuthAgentId { get; set; }

        /// <summary>
        /// 授权应用密钥
        /// </summary>
        public string AuthSecret { get; set; }

        /// <summary>
        /// 默认接收通知的企业微信号
        /// </summary>
        public List<string> DefaultUsernameList { get; set; }

        public static void SaveSettings(Settings settings)
        {
            var filePath = GetPath();
            if (File.Exists(filePath))
            {
                var value = JsonConvert.SerializeObject(settings);
                File.WriteAllText(filePath, value);
            }
        }

        public static Settings GetSettings()
        {
            Settings settings = new Settings();
            var filePath = GetPath();
            if (File.Exists(filePath))
            {
                var fileData = File.ReadAllText(filePath);
                settings = JsonConvert.DeserializeObject<Settings>(fileData);
            }
            else {
                throw new DoveException("check '/Config/CorpWeixinSettings.js' is exists");
            }

            return settings;
        }

        static string GetPath() {
            return WebHelper.MapPath("/Config/CorpWeixinSettings.json");
        }
    }
}
