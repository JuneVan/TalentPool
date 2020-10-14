using System.Collections.Generic;
using System.Threading.Tasks;

namespace Van.TalentPool.Application.Roles
{
    public interface IRoleQuerier
    {
        Task<PaginationOutput<RoleDto>> GetListAsync(PaginationInput input);
        Task<List<RoleDto>> GetRolesAsync();
    }
}
