using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Van.TalentPool.Resumes
{
    public class ResumeComparer : IResumeComparer
    {
        public async Task<List<ResumeCompare>> CompareAsync(ResumeManager manager, Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            return await Task.FromResult<List<ResumeCompare>>(null);
        }
    }
}
