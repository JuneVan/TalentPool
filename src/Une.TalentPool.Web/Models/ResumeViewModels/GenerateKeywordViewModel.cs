using System;

namespace Une.TalentPool.Web.Models.ResumeViewModels
{
    public class GenerateKeywordViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
}
