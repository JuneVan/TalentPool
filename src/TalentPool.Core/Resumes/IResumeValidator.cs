using System.Threading.Tasks;

namespace TalentPool.Resumes
{
    public interface IResumeValidator
    {
        Task ValidateAsync(ResumeManager manager, Resume resume);
    }
}
