using Newtonsoft.Json;
using System;
using System.IO;
using System.Web;

namespace IISDove.Commander.Commands
{
    /// <summary>
    /// Receive the little dove send msgs
    /// [post]
    /// </summary>
    public class ReceiveMessage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ResponseType res = ResponseType.Successful;
            var reqData = "";
            using (StreamReader sr = new StreamReader(context.Request.InputStream))
            {
                reqData = sr.ReadLine();
            }

            if (reqData.IsNotNullOrEmpty())
            {
                try
                {
                    var msg = JsonConvert.DeserializeObject<MessageDto>(reqData);
                    Current.CommanderCommand.ReceiveAndProcess(msg);
                    context.Response.Write(res.ToString());
                    context.Response.End();
                }
                catch (Exception ex)
                {
                    Current.Log(ex, DoveType.Commander);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}