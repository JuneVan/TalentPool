using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Van.TalentPool.Web.Models.DictionaryViewModels
{
    public class CreateOrEditDictionaryModel
    {
        public Guid? Id { get; set; }

        public List<SelectListItem> InjectDictionaries { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<DictionaryItemModel> DictionaryItems { get; set; }
    }

    public class DictionaryItemModel
    { 
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
