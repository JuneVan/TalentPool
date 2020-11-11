using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace TalentPool.Users
{
    public class UserActiveConfirmation : DefaultUserConfirmation<User>
    {
        public override Task<bool> IsConfirmedAsync(UserManager<User> manager, User user)
        {
            return Task.FromResult(user.Confirmed);
        }
    }
}
