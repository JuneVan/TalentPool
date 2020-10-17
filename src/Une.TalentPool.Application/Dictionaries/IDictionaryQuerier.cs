using System.Collections.Generic;
using System.Threading.Tasks;

namespace Une.TalentPool.Application.Dictionaries
{
    public interface IDictionaryQuerier
    {
        Task<PaginationOutput<DictionaryDto>> GetListAsync(PaginationInput input);
        Task<List<DictionaryItemDto>> GetDictionaryAsync(string dictionaryName);

    }
}
