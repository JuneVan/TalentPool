using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Une.TalentPool.Application;
using Une.TalentPool.Application.Users;

namespace Une.TalentPool.EntityFrameworkCore.Queriers
{
    public class UserQuerier : IUserQuerier
    {
        private readonly VanDbContext _context;
        public UserQuerier(VanDbContext context)
        {
            _context = context;
        }
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
                            CreationTime = a.CreationTime
                        };
            if (!string.IsNullOrEmpty(input.Keyword))
                query = query.Where(w => w.Name.Contains(input.Keyword) || w.Surname.Contains(input.Keyword) || w.UserName.Contains(input.Keyword));

            var totalCount = await query.CountAsync();
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var users = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync();

            return new PaginationOutput<UserDto>(totalSize, users);
        }

        public async Task<List<UserSelectItemDto>> GetUsersAsync()
        {
            return await _context.Users
                .Select(s => new UserSelectItemDto() { Id = s.Id, FullName = s.FullName })
                .ToListAsync();
        }
    }
}
