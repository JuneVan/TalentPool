using AutoMapper;
using TalentPool.Evaluations;
using TalentPool.Web.Models.EvaluationViewModels;

namespace TalentPool.Web.Mappings
{
    public class EvaluationMappingProfile:Profile
    {
        public EvaluationMappingProfile()
        {
            CreateMap<CreateOrEditEvaluationViewModel, Evaluation>();
            CreateMap<Evaluation, DeleteEvaluationViewModel>();
            CreateMap<EvaluationSubject, SubjectViewModel>();
            CreateMap<EvaluationQuestion, QuestionViewModel>();
            CreateMap<Evaluation, EvaluationViewModel>();
            CreateMap<EvaluationSubject, CreateOrEditSubjectViewModel>();
            CreateMap<CreateOrEditSubjectViewModel, EvaluationSubject>();
            CreateMap<EvaluationQuestion, CreateOrEditQuestionViewModel>();
            CreateMap<CreateOrEditQuestionViewModel, EvaluationQuestion>();
            CreateMap<EvaluationSubject, DeleteSubjectViewModel>();
            CreateMap<EvaluationQuestion, DeleteQuestionViewModel>();
        }
    }
}
