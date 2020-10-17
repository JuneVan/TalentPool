using System.Threading.Tasks;

namespace Une.TalentPool.Resumes
{
    public interface IResumeValidator
    {
        Task ValidateAsync(ResumeManager manager, Resume resume);
    }
}
