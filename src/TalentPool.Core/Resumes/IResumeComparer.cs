using System.Threading.Tasks;

namespace TalentPool.Resumes
{
    public interface IResumeComparer
    {
        Task  CompareAsync(ResumeManager manager, Resume resume,decimal minSimilarityValue);
    }
}
