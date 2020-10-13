using System.Threading.Tasks;

namespace Van.TalentPool.Infrastructure.Message.Sms
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
