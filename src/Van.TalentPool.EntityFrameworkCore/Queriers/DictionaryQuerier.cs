using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Van.TalentPool.Application;
using Van.TalentPool.Application.Dictionaries;

namespace Van.TalentPool.EntityFrameworkCore.Queriers
{
    public class DictionaryQuerier : IDictionaryQuerier
    {
        private readonly VanDbContext _context;
        public DictionaryQuerier(VanDbContext context)
        {
            _context = context;
        }
        public async Task<PaginationOutput<DictionaryDto>> GetListAsync(PaginationInput input)
        {
            var query = from a in _context.Dictionaries
                        select a;
            var totalCount = await query.CountAsync();
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var dictionaries = await query.OrderByDescending(o => o.Name)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                .Select(s => new DictionaryDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    DisplayName = s.DisplayName
                })
                 .ToListAsync();

            return new PaginationOutput<DictionaryDto>(totalSize, dictionaries);
        }
        public async Task<List<DictionaryItemDto>> GetDictionaryAsync(string dictionaryName)
        {
            if (dictionaryName == null)
                throw new ArgumentNullException(nameof(dictionaryName));

            var dictionaryType = await _context.Dictionaries.Include(i => i.DictionaryItems).FirstOrDefaultAsync(f => f.Name == dictionaryName);
            if (dictionaryType != null && dictionaryType.DictionaryItems != null)
            {
                return dictionaryType.DictionaryItems
                .OrderBy(o => o.Value)
               .Select(s => new DictionaryItemDto()
               {
                   Name = s.Name,
                   Value = s.Value
               }).ToList();
            }
            return null;
        }
    }
}
