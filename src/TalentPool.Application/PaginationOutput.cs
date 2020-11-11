using System.Collections.Generic;

namespace TalentPool.Application
{
    public class PaginationOutput<TDto>
    {
        public int TotalSize { get; set; }
        public IReadOnlyList<TDto> Items { get; set; }
        public PaginationOutput()
        {

        }
        public PaginationOutput(int totalSize, List<TDto> items)
        {
            TotalSize = totalSize;
            Items = items;
        }
    }
}
