using System.Threading.Tasks;

namespace Une.TalentPool.Infrastructure.Message.Email
{
    public  interface IEmailSender
    {
        Task SendEmailAsync(EmailEntry entry);
    }
}
