using System;
using System.Threading.Tasks;

namespace Van.TalentPool.Resumes
{
    public class PhoneNumberValidator : IResumeValidator
    {
        public async Task ValidateAsync(ResumeManager manager, Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));
            if (string.IsNullOrEmpty(resume.PhoneNumber))
                return;
            var owner = await manager.FindByPhoneNumberAsync(resume.PhoneNumber);
            if (owner != null && owner.Id != resume.Id)
                throw new InvalidOperationException($"该手机号码的简历已存在，简历ID：{owner.Id}。");
        }
    }
}
