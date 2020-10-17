using Microsoft.AspNetCore.Identity;

namespace Une.TalentPool.Users
{
    public interface IUserStore : IUserLoginStore<User>,
        IUserClaimStore<User>,
        IUserPasswordStore<User>,
        IUserSecurityStampStore<User>,
        IUserEmailStore<User>,
        IUserLockoutStore<User>,
        IUserPhoneNumberStore<User>,
        IQueryableUserStore<User>,
        IUserTwoFactorStore<User>,
        IUserAuthenticationTokenStore<User>,
        IUserAuthenticatorKeyStore<User>,
        IUserTwoFactorRecoveryCodeStore<User>,
        IUserRoleStore<User>
    {
    }
}
