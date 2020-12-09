using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentPool.Application;
using TalentPool.Application.Dictionaries;

namespace TalentPool.EntityFrameworkCore.Queriers
{
    public class DictionaryQuerier : IDictionaryQuerier
    {
        private readonly TalentDbContext _context;
        private readonly ISignal _signal;
        public DictionaryQuerier(TalentDbContext context, ISignal signal)
        {
            _context = context;
            _signal = signal;
        }
        protected CancellationToken CancellationToken => _signal.Token;

        public async Task<PaginationOutput<DictionaryDto>> GetListAsync(PaginationInput input)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (input == null)
                throw new ArgumentNullException(nameof(input));


            var query = from a in _context.Dictionaries
                        select a;
            var totalCount = await query.CountAsync(CancellationToken);
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
                 .ToListAsync(CancellationToken);

            return new PaginationOutput<DictionaryDto>(totalSize, dictionaries);
        }
        public async Task<List<DictionaryItemDto>> GetDictionaryAsync(string dictionaryName)
        {
            CancellationToken.ThrowIfCancellationRequested(); 
            if (dictionaryName == null)
                throw new ArgumentNullException(nameof(dictionaryName));

            var dictionaryType = await _context.Dictionaries.Include(i => i.DictionaryItems).FirstOrDefaultAsync(f => f.Name == dictionaryName,CancellationToken);
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
