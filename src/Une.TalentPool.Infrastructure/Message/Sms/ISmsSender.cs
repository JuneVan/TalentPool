using System.Threading.Tasks;

namespace Une.TalentPool.Infrastructure.Message.Sms
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
