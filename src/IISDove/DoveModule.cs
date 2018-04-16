using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using U.Utilities.Web;
using IISDove.Events;

namespace IISDove
{
    public class DoveModule : ISenderCommand, ICommanderCommand
    {
        public event EventHandler<ExceuteIISRestartCommandEventArgs> ExceuteIISRestarted;

        #region ISenderCommand
        public void CheckAndSend()
        {
            Stopwatch watch = new Stopwatch();
            var settings = Current.GetSettings();
            string soaUrl = string.Format("{0}{1}",
                                           settings.CommanderHost,
                                          (settings.CommanderHost.EndsWith("/") ? "" : "/") + "Commands/ReceiveMessage.ashx");


            MessageDto msg = new MessageDto();
            msg.Code = settings.Code;
            msg.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            msg.Ip = settings.Ip;
            msg.Name = settings.Name;
            msg.Remark = settings.Remark;
            msg.IISList = new List<IISStatusDto>();

            watch.Start();

            #region Check iis status
            if (settings.IISList != null && settings.IISList.Count > 0)
            {
                foreach (IISStatusDto status in settings.IISList)
                {
                    WebClient client = new WebClient();
                    try
                    {
                        //首页+静态页双验证
                        if (status.Localhost.IsNotNullOrEmpty())
                        {
                            byte[] pageDatas = client.DownloadData(status.Localhost);
                        }
                        if (status.AliveUrl.IsNotNullOrEmpty())
                        {
                            byte[] pageDatas2 = client.DownloadData(status.AliveUrl);
                        }
                        status.StatusCode = (int)StatusCode.Ok;
                    }
                    catch (Exception ex)
                    {
                        status.StatusCode = (int)StatusCode.SiteOccurError;
                        Current.Log(ex, DoveType.Sender);
                    }

                    msg.IISList.Add(status);
                }
            }
            #endregion

            watch.Stop();
            msg.CheckElapsedTime = watch.Elapsed.Seconds;

            #region Send
            var requestJson = JsonConvert.SerializeObject(msg);
            HttpContent httpContent = new StringContent(requestJson);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var httpClient = new HttpClient();

            //Current.Log(requestJson); //test
            try
            {
                httpClient.PostAsync(soaUrl, httpContent);
            }
            catch (Exception ex)
            {
                Current.Log(ex);
            }
            #endregion
        }
        #endregion

        public void ReceiveAndProcess(MessageDto msg)
        {

            if (msg != null)
            {
                DoveSenderDto sender = new DoveSenderDto();
                sender.Code = msg.Code;
                sender.Name = msg.Name;
                sender.Remark = msg.Remark;
                sender.Ip = msg.Ip;
                sender.LastSendTime = msg.SendTime;
                sender.LastCheckElapsedTime = msg.CheckElapsedTime;
                sender.IISStatus = msg.IISList;
                //sender.IsAutoExecuteCommand=
                #region process iis restart
                if (sender.IISStatus != null)
                {
                    foreach (var iis in sender.IISStatus)
                    {
                        if (iis.StatusCode == (int)StatusCode.SiteOccurError && DateTime.Now > sender.LastSendTime.ToDateTime().AddSeconds(30))
                        {
                            UIISManageClient.Restart(sender.Ip, iis.SiteName);
                            this.ExceuteIISRestarted(this, new ExceuteIISRestartCommandEventArgs(sender, iis.SiteName));
                        }
                    }
                }
                #endregion

                //save
                SaveSenderInfo(sender);
            }
        }

        public void SaveSenderInfo(DoveSenderDto doveSender)
        {
            try
            {
                if (doveSender == null)
                {
                    throw new Exception("doveSender is null");
                }

                var list = GetAllSenders();
                if (list == null)
                {
                    list = new List<DoveSenderDto>();
                }

                var source = list.Where(x => x.Code == doveSender.Code).FirstOrDefault();

                if (source != null)
                {
                    //update
                    source.LastCheckElapsedTime = doveSender.LastCheckElapsedTime;
                    source.LastSendTime = doveSender.LastSendTime;
                    source.Remark = doveSender.Remark;
                    source.Name = doveSender.Name;
                    source.Ip = doveSender.Ip;
                    source.TotalOfSending = doveSender.TotalOfSending;
                    source.IISStatus = doveSender.IISStatus;
                    source.TotalOfSending++;
                }
                else
                {
                    doveSender.TotalOfSending = 1;
                    list.Add(doveSender);
                }

                //save
                if (list.Count > 0)
                {
                    using (FileStream fs = new FileStream(GetDoveSenderFilePath(), FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            var value = JsonConvert.SerializeObject(list);
                            sw.Write(value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Current.Log(ex, DoveType.Commander);
            }
        }

        public IList<DoveSenderDto> GetAllSenders()
        {
            IList<DoveSenderDto> list = new List<DoveSenderDto>();
            try
            {
                using (FileStream fs = new FileStream(GetDoveSenderFilePath(), FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string content = sr.ReadToEnd();
                        if (content.IsNotNullOrEmpty())
                        {
                            list = JsonConvert.DeserializeObject<IList<DoveSenderDto>>(content);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Current.Log(ex, DoveType.Commander);
            }
            return list.OrderBy(x => x.Code).ToList();
        }

        string GetDoveSenderFilePath()
        {
            var filePath = WebHelper.MapPath(string.Format("/app_data/dove_senders.txt", DateTime.Now.ToString("yyyyMMdd")));
            if (!File.Exists(filePath))
            {
                var file = File.Create(filePath);
                file.Close();
            }

            return filePath;
        }
    }
}
