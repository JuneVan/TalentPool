using System;
using System.Collections.Generic;
using Van.TalentPool.Entities;

namespace Van.TalentPool.DailyStatistics
{
    public class DailyStatistic : Entity, ICreationAudited, IModificationAudited
    {
        /// <summary>
        ///  平台
        /// </summary>
        public virtual string Platform { get; set; }
        /// <summary>
        /// 统计的日期
        /// </summary>
        public virtual DateTime Date { get; set; }

        public virtual string Description { get; set; }

        public virtual ICollection<DailyStatisticItem>  Items { get; set; }

        public virtual Guid CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual Guid? LastModifierUserId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
    }
}
