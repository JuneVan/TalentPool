using System;
using Van.TalentPool.Entities;

namespace Van.TalentPool.DailyStatistics
{
    public class DailyStatisticItem : Entity
    {
        // 职位
        public virtual string JobName { get; set; }
        // 更新数量
        public virtual int UpdateCount { get; set; }
        // 下载数量
        public virtual int DownloadCount { get; set; }

        public virtual Guid DailyStatisticId { get; set; }
    }
}
