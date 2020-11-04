using System;
using TalentPool.Entities;

namespace TalentPool.Investigations
{
    public class Investigation : Entity, ICreationAudited, IModificationAudited, IDeletionAudited
    {
        /// <summary>
        /// 简历Id
        /// </summary>
        public Guid ResumeId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } 
        /// <summary>
        /// 调查日期（默认为创建时间，但当电话未接通或者考虑情况下，可以通过更新该字段，来更新到当日的调查记录列表）
        /// </summary>
        public DateTime InvestigateDate { get; set; }
      
        /// <summary>
        /// 调查状态
        /// </summary>
        public virtual InvestigationStatus Status { get; set; }
        /// <summary>
        /// 标记（合适或不合适  ，因为涉及工资是否合理、是否愿意出差、技术评测是否合格多项判断，无法使用程序逻辑，需人工手动标记，默认为空）
        /// </summary>
        public virtual bool? IsQualified { get; set; }
        /// <summary>
        /// 记录是否合适的原因
        /// </summary>
        public virtual string QualifiedRemark { get; set; }

        /// <summary>
        /// 联系情况
        /// </summary>
        public virtual bool? IsConnected { get; set; }
        /// <summary>
        /// 未接通的备注信息
        /// </summary>
        public virtual string UnconnectedRemark { get; set; }


        /*          调查内容           */

        /// <summary>
        /// 是否可出差(枚举数据可为空，因为未接通情况下无法记录值)
        /// </summary>
        public virtual AcceptTravelStatus? AcceptTravelStatus { get; set; }
        /// <summary>
        /// 不出差的理由
        /// </summary>
        public virtual string NotAcceptTravelReason { get; set; }
        /// <summary>
        /// 期望薪水
        /// </summary>
        public virtual string ExpectedSalary { get; set; }
        /// <summary>
        /// 工作状态
        /// </summary>
        public virtual WorkState? WorkState { get; set; }
        /// <summary>
        /// 预期可上班日期
        /// </summary>
        public virtual string ExpectedDate { get; set; }

        /// <summary>
        /// 是否接受现场面试
        /// </summary>
        public virtual bool? IsAcceptInterview { get; set; }
        /// <summary>
        /// 预期可现场面试日期
        /// </summary>
        public virtual string ExpectedInterviewDate { get; set; }
        /// <summary>
        /// 预期可电话面试日期
        /// </summary>
        public virtual string ExpectedPhoneInterviewDate { get; set; }
        /// <summary>
        /// 调查信息
        /// </summary>
        public virtual string Information { get; set; }
        /// <summary>
        /// 技术评测内容
        /// </summary>
        public virtual string Evaluation { get; set; }
        /// <summary>
        /// 评测时间（未来时间则为预约时间）
        /// </summary>
        public virtual DateTime? EvaluationTime { get; set; }
        /// <summary>
        /// 居住地城市
        /// </summary>
        public virtual string CityOfResidence { get; set; }
        /// <summary>
        /// 户籍地城市
        /// </summary>
        public virtual string CityOfDomicile { get; set; }


       


        public virtual Guid CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual Guid? LastModifierUserId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? DeleterUserId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual bool IsDeleted { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
