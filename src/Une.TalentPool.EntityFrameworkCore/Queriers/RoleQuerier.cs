using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Une.TalentPool.Application;
using Une.TalentPool.Application.Roles;

namespace Une.TalentPool.EntityFrameworkCore.Queriers
{
    public class RoleQuerier : IRoleQuerier
    {
        private readonly TalentDbContext _context;
        protected readonly ICancellationTokenProvider _tokenProvider;
        public RoleQuerier(TalentDbContext context, ICancellationTokenProvider tokenProvider)
        {
            _context = context;
            _tokenProvider = tokenProvider;
        }
        protected CancellationToken CancellationToken => _tokenProvider.Token;

        public async Task<PaginationOutput<RoleDto>> GetListAsync(PaginationInput input)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var query = from a in _context.Roles
                        select new RoleDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            DisplayName = a.DisplayName,
                            Active = a.Active,
                            Protected = a.Protected
                        };
            var totalCount = await query.CountAsync(CancellationToken);
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var roles = await query.OrderBy(o => o.Name)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync(CancellationToken);

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
