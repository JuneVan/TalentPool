using System.Threading.Tasks;

namespace TalentPool.Infrastructure.Message.Sms
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
