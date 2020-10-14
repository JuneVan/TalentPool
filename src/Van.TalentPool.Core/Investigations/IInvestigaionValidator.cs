using System.Threading.Tasks;

namespace Van.TalentPool.Investigations
{
    public interface IInvestigaionValidator
    {
        Task ValidateAsync(InvestigationManager manager, Investigation investigation);
    }
}
