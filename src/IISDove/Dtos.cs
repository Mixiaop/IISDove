using System;
using System.Collections.Generic;

namespace IISDove
{
    [Serializable]
    public class IISStatusDto
    {
        public string SiteName { get; set; } = "";
        public string Localhost { get; set; } = "";
        public string AliveUrl { get; set; } = "";

        public int StatusCode { get; set; } = 1;

        /// <summary>
        /// 最近被执行重启的时间
        /// </summary>
        public string LastRestartTime { get; set; } = "";
    }

    [Serializable]
    public class MessageDto
    {
        /// <summary>
        /// dove's code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// dove's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// dove's remark
        /// </summary>
        public string Remark { get; set; }

        public string Ip { get; set; }

        public string SendTime { get; set; }

        public int CheckElapsedTime { get; set; }

        public List<IISStatusDto> IISList { get; set; }
    }

    /// <summary>
    /// Record how many dove.senders, monitoring iis site status
    /// </summary>
    public class DoveSenderDto {
        public DoveSenderDto() {
        }
        /// <summary>
        /// dove's code
        /// </summary>
        public int Code { get; set; }

        public string Name { get; set; }

        public string Ip { get; set; }

        public string Remark { get; set; }

        public string LastSendTime { get; set; } 

        public int LastCheckElapsedTime { get; set; }

        public int TotalOfSending { get; set; }

        public List<IISStatusDto> IISStatus { get; set; } = new List<IISStatusDto>();

        public string LastExecuteCommand { get; set; }
    }
}
