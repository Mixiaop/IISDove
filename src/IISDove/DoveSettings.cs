using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace IISDove
{
    [Serializable]
    public class DoveSettings
    {
        /// <summary>
        /// 1 = sender
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 1000 = 1's
        /// </summary>
        public int SendInterval { get; set; }

        /// <summary>
        /// dove's code
        /// </summary>
        public int Code { get; set; }

        public string Name { get; set; }

        public string Ip { get; set; }

        public string Remark { get; set; }

        public string CommanderHost { get; set; }

        public List<IISStatusDto> IISList { get; set; }

        public bool IsAutoExecuteCommand { get; set; }

        public static void SaveSettings(DoveSettings settings, DoveType type = DoveType.Sender) {
            if (type == DoveType.Sender)
            {
                var filePath = AppDomain.CurrentDomain.BaseDirectory + @"\DoveSettings.json";
                if (File.Exists(filePath))
                {
                    var value = JsonConvert.SerializeObject(settings);
                    File.WriteAllText(filePath, value);
                }
            }
            else
            {
                throw new DoveException("check type,current just sender settings[1]");
            }
        }

        public static DoveSettings GetSettings(DoveType type)
        {
            DoveSettings settings = new DoveSettings();
            if (type == DoveType.Sender)
            {

                var filePath = AppDomain.CurrentDomain.BaseDirectory + @"\DoveSettings.json";
                if (File.Exists(filePath))
                {
                    var fileData = File.ReadAllText(filePath);
                    settings = JsonConvert.DeserializeObject<DoveSettings>(fileData);
                }
                else
                {
                    throw new DoveException("not found settings" + filePath);
                }

                return settings;
            }
            else
            {
                throw new DoveException("check type,current just sender settings[1]");
            }
        }
    }
}
