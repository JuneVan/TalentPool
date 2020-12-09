using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentPool.Application;
using TalentPool.Application.Users;

namespace TalentPool.EntityFrameworkCore.Queriers
{
    public class UserQuerier : IUserQuerier
    {
        private readonly TalentDbContext _context;
        private readonly ISignal _signal;
        public UserQuerier(TalentDbContext context,
            ISignal signal)
        {
            _context = context;
            _signal = signal;
        }
        protected CancellationToken CancellationToken => _signal.Token;

        public async Task<PaginationOutput<UserDto>> GetListAsync(QueryUserInput input)
        {
            var query = from a in _context.Users
                        select new UserDto()
                        {
                            Id = a.Id,
                            UserName = a.UserName,
                            Name = a.Name,
                            Surname = a.Surname,
                            Email = a.Email,
                            EmailConfirmed = a.EmailConfirmed,
                            PhoneNumber = a.PhoneNumber,
                            PhoneNumberConfirmed = a.PhoneNumberConfirmed,
                            Photo = a.Photo,
                            Active = a.Confirmed,
                            CreationTime = a.CreationTime,
                            Protected = a.Protected
                        };
            if (!string.IsNullOrEmpty(input.Keyword))
                query = query.Where(w => w.Name.Contains(input.Keyword) || w.Surname.Contains(input.Keyword) || w.UserName.Contains(input.Keyword));

            var totalCount = await query.CountAsync(CancellationToken);
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var users = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync(CancellationToken);

            return new PaginationOutput<UserDto>(totalSize, users);
        }

        public async Task<List<UserSelectItemDto>> GetUsersAsync()
        {
            var query = from a in _context.Users
                        where a.Confirmed == true
                        select new UserSelectItemDto
                        {
                           Id= a.Id,
                          FullName=  a.FullName 
                        };
            return await query.ToListAsync(CancellationToken);
        }
    }
}
