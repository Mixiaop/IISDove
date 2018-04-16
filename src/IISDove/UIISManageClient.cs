using System.Text;
using U.Utilities.Net;

namespace IISDove
{
    public class UIISManageClient
    {
        protected static string Port = "8010";
        public static void Restart(string ip, string siteName)
        {
            var host = string.Format("http://{0}:{1}", ip, Port);

            if (siteName.IsNotNullOrEmpty())
            {
                if (!host.EndsWith("/"))
                    host += "/";

                if (!host.Contains("http://"))
                    host = "http://" + host;

                var soaUrl = host + "SiteAutoStart.aspx";
                soaUrl += "?siteName=" + siteName;

                var message = WebRequestHelper.HttpGet(soaUrl, Encoding.GetEncoding("utf-8"));
            }
        }
    }
}
