using System.Collections.Generic;
using System.Threading.Tasks;

namespace Une.TalentPool.Application.Users
{
    public interface IUserQuerier
    {
        Task<PaginationOutput<UserDto>> GetListAsync(QueryUserInput input);
        Task<List<UserSelectItemDto>> GetUsersAsync();
    }
}
