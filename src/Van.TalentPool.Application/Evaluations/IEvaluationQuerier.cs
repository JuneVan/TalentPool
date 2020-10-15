using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Van.TalentPool.Application.Evaluations
{
    public interface IEvaluationQuerier
    {
        Task<PaginationOutput<EvaluationDto>> GetListAsync(QueryEvaluationInput input);
        Task<PaginationOutput<SubjectDto>> GetSubjectsAsync(QuerySubjectInput input);
        Task<PaginationOutput<QuestionDto>> GetQuestionsAsync(QueryQuestionInput input);

        Task<List<EvaluationSelectItemDto>> GetEvaluationsAsync(Guid jobId);
    }
}
