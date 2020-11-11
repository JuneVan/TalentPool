using System;
using System.Threading.Tasks;

namespace TalentPool.Resumes
{
    public class PlatformValidator : IResumeValidator
    {
        public async Task ValidateAsync(ResumeManager manager, Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            if (string.IsNullOrEmpty(resume.PlatformId))
                return;
            var owner = await manager.FindByPlatformAsync(resume.PlatformId);
            if (owner != null && owner.Id != resume.Id)
                throw new InvalidOperationException($"该招聘平台ID的简历已存在，简历ID：{owner.Id}。");
            // 平台id为手机号码时检测重复性
            owner = await manager.FindByPhoneNumberAsync(resume.PlatformId);
            if (owner != null && owner.Id != resume.Id)
                throw new InvalidOperationException($"该手机号码的简历已存在，简历ID：{owner.Id}。");
        }
    }
}
