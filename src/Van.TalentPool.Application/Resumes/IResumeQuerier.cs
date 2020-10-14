using System;
using System.Threading.Tasks;

namespace Van.TalentPool.Application.Resumes
{
    public interface IResumeQuerier
    {
        Task<PaginationOutput<ResumeDto>> GetListAsync(QueryResumeInput input);
        Task<ResumeDetailDto> GetResumeAsync(Guid id);
    }
}
