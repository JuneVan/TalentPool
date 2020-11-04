using System.Threading.Tasks;

namespace TalentPool.Investigations
{
    public interface IInvestigaionValidator
    {
        Task ValidateAsync(InvestigationManager manager, Investigation investigation);
    }
}
