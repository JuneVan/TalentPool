using AutoMapper;
using Van.TalentPool.Dictionaries;
using Van.TalentPool.Web.Models.DictionaryViewModels;

namespace Van.TalentPool.Web.Profiles
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
