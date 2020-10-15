using System.Threading.Tasks;

namespace Van.TalentPool.Application.Interviews
{
    public interface IInterviewQuerier
    {
        Task<PaginationOutput<InterviewDto>> GetListAsync(QueryInterviewInput input);
    }
}
