using System.Threading.Tasks;

namespace Van.TalentPool.Application.Investigations
{
    public interface IInvestigationQuerier
    {
        //分页列表
        Task<PaginationOutput<InvestigationDto>> GetListAsync(QueryInvestigaionInput input);
    }
}
