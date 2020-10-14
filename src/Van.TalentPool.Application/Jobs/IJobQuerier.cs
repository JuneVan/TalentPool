using System.Collections.Generic;
using System.Threading.Tasks;

namespace Van.TalentPool.Application.Jobs
{
    public interface IJobQuerier
    {
        Task<PaginationOutput<JobDto>> GetListAsync(QueryJobInput input);
        Task<List<JobSelectItemDto>> GetJobsAsync();
    }
}
