using System.Threading.Tasks;

namespace Une.TalentPool.Resumes
{
    public interface IResumeComparer
    {
        Task  CompareAsync(ResumeManager manager, Resume resume,decimal minSimilarityValue);
    }
}
