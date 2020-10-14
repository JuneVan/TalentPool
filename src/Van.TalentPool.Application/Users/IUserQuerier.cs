using System.Collections.Generic;
using System.Threading.Tasks;

namespace Van.TalentPool.Application.Users
{
    public interface IUserQuerier
    {
        Task<PaginationOutput<UserDto>> GetListAsync(QueryUserInput input);
        Task<List<UserSelectItemDto>> GetUsersAsync();
    }
}
