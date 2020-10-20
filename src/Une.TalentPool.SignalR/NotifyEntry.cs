using System;
using System.Collections.Generic;

namespace Une.TalentPool.SignalR
{
    public class NotifyEntry
    { 
        /// <summary>
        /// 接受对象
        /// </summary>
        public List<Guid> ReceiverUserIds { get; set; } = new List<Guid>();
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
