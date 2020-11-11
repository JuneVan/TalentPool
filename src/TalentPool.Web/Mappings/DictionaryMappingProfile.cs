using AutoMapper;
using TalentPool.Dictionaries;
using TalentPool.Web.Models.DictionaryViewModels;

namespace TalentPool.Web.Mappings
{
    public class DictionaryMappingProfile: Profile
    {
        public DictionaryMappingProfile()
        { 
            CreateMap<Dictionary, CreateOrEditDictionaryModel>(); 
            CreateMap<CreateOrEditDictionaryModel, Dictionary>(); 
            CreateMap<DictionaryItem, DictionaryItemModel>(); 
            CreateMap<DictionaryItemModel, DictionaryItem>();  
        }
    }
}
