using AutoMapper;
using Une.TalentPool.Dictionaries;
using Une.TalentPool.Web.Models.DictionaryViewModels;

namespace Une.TalentPool.Web.Mappings
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
