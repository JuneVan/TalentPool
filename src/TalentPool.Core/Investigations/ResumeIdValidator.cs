using System;
using System.Threading.Tasks;

namespace TalentPool.Investigations
{
    public class ResumeIdValidator : IInvestigaionValidator
    {
        public async Task ValidateAsync(InvestigationManager manager, Investigation investigation)
        {
            if (investigation == null)
                throw new ArgumentNullException(nameof(investigation));
            var owner = await manager.FindByResumeIdAsync(investigation.ResumeId);
            if (owner != null && owner.Id != investigation.Id)
                throw new InvalidOperationException($"已经存在简历的意向调查记录，调查ID：{owner.Id}。");

        }
    }
}
