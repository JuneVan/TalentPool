using System.Threading.Tasks;

namespace Van.TalentPool.Resumes
{
    public interface IResumeValidator
    {
        Task ValidateAsync(ResumeManager manager, Resume resume);
    }
}
