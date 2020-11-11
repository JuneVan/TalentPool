using System.Threading.Tasks;

namespace TalentPool.Infrastructure.Message.Sms
{
    public class SmsSender : ISmsSender
    {
        public Task SendSmsAsync(string number, string message)
        {
            return Task.FromResult(0);
        }
    }
}
