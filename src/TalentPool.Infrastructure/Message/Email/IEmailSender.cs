using System.Threading.Tasks;

namespace TalentPool.Infrastructure.Message.Email
{
    public  interface IEmailSender
    {
        Task SendEmailAsync(EmailEntry entry);
    }
}
