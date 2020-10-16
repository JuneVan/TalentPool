using AutoMapper;
using Van.TalentPool.Evaluations;
using Van.TalentPool.Web.Models.EvaluationViewModels;

namespace Van.TalentPool.Web.Profiles
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
        }
    }
}
