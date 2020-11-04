using System;
using System.Collections.Generic;

namespace TalentPool.Web.Models.EvaluationViewModels
{
    public class EvaluationViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        // 创建人姓名
        public string CreatedName { get; set; }
        // 创建时间
        public DateTime CreatedTime { get; set; }

        // 修改人姓名
        public string ModifiedName { get; set; }
        // 修改时间
        public DateTime ModifiedTime { get; set; }

        public List<SubjectViewModel> Subjects { get; set; }
    }

    public class SubjectViewModel
    {
        public Guid Id { get; set; }
        public string Keyword { get; set; }
        public int Weight { get; set; }
        public string Description { get; set; }

        public List<QuestionViewModel> Questions { get; set; }

        // 创建人姓名
        public string CreatedName { get; set; }
        // 创建时间
        public DateTime CreatedTime { get; set; }
    }
    public class QuestionViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string ReferenceAnswer { get; set; }

        // 创建人姓名
        public string CreatedName { get; set; }
        // 创建时间
        public DateTime CreatedTime { get; set; }
    }
}
