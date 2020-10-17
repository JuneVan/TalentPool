using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Une.TalentPool.Web.Models.DictionaryViewModels
{
    public class CreateOrEditDictionaryModel
    {
        public Guid? Id { get; set; }

        public List<SelectListItem> InjectDictionaries { get; set; }

        [Required(ErrorMessage ="请选择字典名称。")]
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<DictionaryItemModel> DictionaryItems { get; set; }
    }

    public class DictionaryItemModel
    { 
        public string Name { get; set; }
        [Required(ErrorMessage = "字典的值不能为空。")]
        public int Value { get; set; }
    }
}
