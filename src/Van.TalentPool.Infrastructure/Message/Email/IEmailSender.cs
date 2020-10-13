using System.Threading.Tasks;

namespace Van.TalentPool.Infrastructure.Message.Email
{
    public  interface IEmailSender
    {
        Task SendEmailAsync(EmailEntry entry);
    }
}
