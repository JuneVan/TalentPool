using AutoMapper;
using Une.TalentPool.Evaluations;
using Une.TalentPool.Web.Models.EvaluationViewModels;

namespace Une.TalentPool.Web.Mappings
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
