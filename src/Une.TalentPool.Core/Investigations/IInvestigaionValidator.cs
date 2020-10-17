using System.Threading.Tasks;

namespace Une.TalentPool.Investigations
{
    public interface IInvestigaionValidator
    {
        Task ValidateAsync(InvestigationManager manager, Investigation investigation);
    }
}
