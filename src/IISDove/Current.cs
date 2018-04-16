using System;
using System.IO;
using U.Utilities.Web;
using IISDove.CorpWeixin;
namespace IISDove
{
    public static class Current
    {
        static Current()
        {
            _senderCommand = new DoveModule();
            _commanderCommand = new DoveModule();
            CommanderCommand.ExceuteIISRestarted += CommanderCommand_ExceuteIISRestarted;
        }

        #region Events
        private static void CommanderCommand_ExceuteIISRestarted(object sender, Events.ExceuteIISRestartCommandEventArgs e)
        {
            string message = string.Format("【来自鸽子司令的消息】：{0}_{1}_{2}执行了重启IIS站点的命令。", (e.Sender.Code + "_" + e.Sender.Name), e.Sender.Ip, e.SiteName);
            NotifyService.SendMessage(message);
        }
        #endregion

        private static ISenderCommand _senderCommand;
        public static ISenderCommand SenderCommand => _senderCommand;

        private static ICommanderCommand _commanderCommand;
        public static ICommanderCommand CommanderCommand => _commanderCommand;

        public static DoveSettings GetSettings(DoveType type = DoveType.Sender)
        {
            return DoveSettings.GetSettings(type);
        }

        #region Log
        public static void Log(string message, DoveType type = DoveType.Sender) {
            LogMessage(message, type);
        }

        public static void Log(Exception ex, DoveType type = DoveType.Sender) {
            LogMessage("出错了：" + ex.Message, type);
        }

        private static void LogMessage(string message, DoveType type = DoveType.Sender)
        {
            string path = string.Format(GetLogPath(type) + @"\log_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                FileStream fs;
                fs = File.Create(path);
                fs.Close();
            }

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "   " + message);
                }
            }
        }

        private static string GetLogPath(DoveType type = DoveType.Sender)
        {
            string path = "";
            if (type == DoveType.Sender)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\logs";
            }
            else
            {
                path = WebHelper.MapPath("/app_data/logs");
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
        #endregion
    }
}
