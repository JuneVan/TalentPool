using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Une.TalentPool.Users;

namespace Une.TalentPool.SignalR
{
    public class NotifyHub : Hub
    {
        private readonly IUserIdentifier _userIdentifier;
        private readonly UserManager _userManager;
        public NotifyHub(IUserIdentifier userIdentifier,
            UserManager userManager)
        {
            _userIdentifier = userIdentifier;
            _userManager = userManager;
        }
        public async override Task OnConnectedAsync()
        {
            if (_userIdentifier.UserId.HasValue)
            {
                var user = await _userManager.FindByIdAsync(_userIdentifier.UserId.ToString());
                if (user != null)
                    await _userManager.ChangeOnlineAsync(user, Context.ConnectionId);
            }
        }
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            if (_userIdentifier.UserId.HasValue)
            {

                var user = await _userManager.FindByIdAsync(_userIdentifier.UserId.ToString());
                if (user != null)
                    await _userManager.ChangeOnlineAsync(user, null);

            }
        }
    }
}
