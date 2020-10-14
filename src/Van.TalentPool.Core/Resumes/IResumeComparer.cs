using System.Collections.Generic;
using System.Threading.Tasks;

namespace Van.TalentPool.Resumes
{
    public interface IResumeComparer
    {
        Task<List<ResumeCompare>> CompareAsync(ResumeManager manager, Resume resume);
    }
}
