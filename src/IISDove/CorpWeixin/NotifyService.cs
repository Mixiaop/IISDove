using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using U.Utilities.Net;

namespace IISDove.CorpWeixin
{
    public class NotifyService
    {
        private static GetCorpWexinAccessTokenOutput GetAccessToken()
        {
            GetCorpWexinAccessTokenOutput res = new GetCorpWexinAccessTokenOutput();
            var settings = Settings.GetSettings();
            if (settings.CorpId.IsNotNullOrEmpty() && settings.AuthSecret.IsNotNullOrEmpty())
            {
                string url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", settings.CorpId, settings.AuthSecret);
                var response = WebRequestHelper.HttpGet(url, Encoding.GetEncoding("utf-8"));

                if (response.IsNotNullOrEmpty())
                    res = JsonConvert.DeserializeObject<GetCorpWexinAccessTokenOutput>(response);
            }

            return res;
        }

        private static GetCorpWeixinUserIdOutput GetUserId(string code)
        {
            GetCorpWeixinUserIdOutput res = new GetCorpWeixinUserIdOutput();
            GetCorpWexinAccessTokenOutput token = GetAccessToken();
            if (token.errmsg == "ok")
            {
                string accessToken = token.access_token;

                if (accessToken.IsNotNullOrEmpty() && code.IsNotNullOrEmpty())
                {
                    var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}", accessToken, code);
                    var response = WebRequestHelper.HttpGet(url, Encoding.GetEncoding("utf-8"));

                    if (response.IsNotNullOrEmpty())
                        res = JsonConvert.DeserializeObject<GetCorpWeixinUserIdOutput>(response);
                }
            }
            return res;
        }

        public static CorpWeixinSendMessageOutput SendMessage(string content)
        {
            CorpWeixinSendMessageOutput res = new CorpWeixinSendMessageOutput();

            var settings = Settings.GetSettings();
            List<string> userIdList = settings.DefaultUsernameList;

            GetCorpWexinAccessTokenOutput token = GetAccessToken();
            if (token.errmsg == "ok")
            {
                string accessToken = token.access_token;

                var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", accessToken);

                CorpWeixinSendMessageDto input = new CorpWeixinSendMessageDto();

                if (userIdList != null)
                {
                    userIdList.ForEach((userId) => {
                        input.touser += userId + "|";
                    });

                    if (input.touser.IsNotNullOrEmpty())
                        input.touser = input.touser.TrimEnd("|");
                }
                input.agentid = settings.AuthAgentId;
                input.text.content = content;
                var data = JsonConvert.SerializeObject(input);

                var response = WebRequestHelper.HttpPost(url, data);
                if (response.IsNotNullOrEmpty())
                    res = JsonConvert.DeserializeObject<CorpWeixinSendMessageOutput>(response);
            }
            return res;
        }
    }
}
