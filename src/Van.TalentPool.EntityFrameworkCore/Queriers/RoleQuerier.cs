using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Van.TalentPool.Application;
using Van.TalentPool.Application.Roles;

namespace Van.TalentPool.EntityFrameworkCore.Queriers
{
    public class RoleQuerier : IRoleQuerier
    {
        private readonly VanDbContext _context;
        public RoleQuerier(VanDbContext context)
        {
            _context = context;
        }
        public async Task<PaginationOutput<RoleDto>> GetListAsync(PaginationInput input)
        {
            var query = from a in _context.Roles
                        select new RoleDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            DisplayName = a.DisplayName,
                            Active = a.Active,
                            Protected = a.Protected
                        };
            var totalCount = await query.CountAsync();
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var roles = await query.OrderBy(o => o.Name)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync();

            return new PaginationOutput<RoleDto>(totalSize, roles);
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            return await _context.Roles.Where(w => w.Active)
                .Select(s => new RoleDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    DisplayName = s.DisplayName,
                    Description = s.Description
                })
                .ToListAsync();
        }
    }
}
