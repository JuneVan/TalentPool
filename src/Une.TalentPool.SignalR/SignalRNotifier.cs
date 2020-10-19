using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Une.TalentPool.Users;

namespace Une.TalentPool.SignalR
{
    public class SignalRNotifier : ISignalRNotifier
    {
        private readonly IUserIdentifier _userIdentifier;
        private readonly IHubContext<NotifyHub> _hubContext;
        private readonly UserManager _userManager;
        private readonly ILogger _logger;
        public SignalRNotifier(IUserIdentifier userIdentifier,
            IHubContext<NotifyHub> hubContext,
            UserManager userManager,
            Logger<SignalRNotifier> logger)
        {
            _userIdentifier = userIdentifier;
            _hubContext = hubContext;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task NotifyAsync(NotifyEntry notifyEntry)
        {
            if (notifyEntry.ReceiverUserIds == null || notifyEntry.ReceiverUserIds.Count <= 0 || string.IsNullOrEmpty(notifyEntry.Content))
                return;

            try
            {
                var sender = await _userManager.FindByIdAsync(_userIdentifier.UserId.ToString());
                foreach (var item in notifyEntry.ReceiverUserIds)
                {
                    var receiver = await _userManager.FindByIdAsync(item.ToString());
                    if (receiver != null && !string.IsNullOrEmpty(receiver.ConnectionId))//在线状态才有ConnectionId
                    {
                        _logger.LogInformation($"{receiver.ConnectionId}:{notifyEntry.Content}");
                        await _hubContext.Clients.Client(receiver.ConnectionId).SendAsync("Notify", new
                        {
                            Message = notifyEntry.Content,
                            UserName = $"{sender.FullName}",
                            HeadImage = sender.Photo
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "发送通知系统异常。");
            }
        }
    }
}
