using System.Collections.Generic;
using System.Threading.Tasks;

namespace Une.TalentPool.Resumes
{
    public interface IResumeComparer
    {
        Task<List<ResumeCompare>> CompareAsync(ResumeManager manager, Resume resume);
    }
}
