namespace IISDove.CorpWeixin
{
    /// <summary>
    /// 企业微信接口默认响应
    /// </summary>
    public class CorpWeixinResponseDto
    {
        public CorpWeixinResponseDto()
        {
            errcode = 404;
        }

        public int errcode { get; set; }

        public string errmsg { get; set; }

        public bool IsSuccess()
        {
            return errcode == 0;
        }
    }

    /// <summary>
    /// 应用支持推送文本、图片、视频、文件、图文等类型。
    /// 
    /// 请求示例
    /// {
    /// "touser" : "UserID1|UserID2|UserID3",
    /// "toparty" : "PartyID1|PartyID2",
    /// "totag" : "TagID1 | TagID2",
    /// "msgtype" : "text",
    /// "agentid" : 1,
    /// "text" : {
    ///        "content" : "你的快递已到，请携带工卡前往邮件中心领取。\n出发前可查看<a href=\"http://work.weixin.qq.com\">邮件中心视频实况</a>，聪明避开排队。"
    ///  },
    /// "safe":0
    /// }
    /// </summary>
    public class CorpWeixinSendMessageDto
    {
        public CorpWeixinSendMessageDto()
        {
            safe = 0;
            msgtype = "text";
            text = new CorpWeixinSendMessageContentDto();
        }

        public string touser { get; set; }

        public string toparty { get; set; }

        public string totag { get; set; }

        public string msgtype { get; set; }

        public string agentid { get; set; }

        public CorpWeixinSendMessageContentDto text { get; set; }

        public int safe { get; set; }
    }

    public class CorpWeixinSendMessageContentDto
    {
        public string content { get; set; }
    }

    /// <summary>
    /// 发送消息模型
    /// </summary>
    public class CorpWeixinSendMessageOutput : CorpWeixinResponseDto
    {
        /// <summary>
        /// "userid1|userid2", // 不区分大小写，返回的列表都统一转为小写
        /// </summary>
        public string invaliduser { get; set; }

        public string invalidparty { get; set; }

        public string invalidtag { get; set; }
    }

    public class GetCorpWeixinUserIdOutput : CorpWeixinResponseDto
    {
        public string UserId { get; set; }

        public string DeviceId { get; set; }
    }

    /// <summary>
    /// 获取AccessToken
    /// </summary>
    public class GetCorpWexinAccessTokenOutput : CorpWeixinResponseDto
    {
        public string access_token { get; set; }

        /// <summary>
        /// 过期时间（秒）
        /// 正常情况下AccessToken有效期为7200秒，有效期内重复获取返回相同结果。
        /// </summary>
        public int expires_in { get; set; }
    }
}
