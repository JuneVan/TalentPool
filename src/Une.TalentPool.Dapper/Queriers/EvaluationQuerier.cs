using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Une.TalentPool.Application;
using Une.TalentPool.Application.Evaluations;

namespace Une.TalentPool.Dapper.Queriers
{
    public class EvaluationQuerier : IEvaluationQuerier
    {
        private TalentDbContext _context;
        public EvaluationQuerier(TalentDbContext context)
        {
            _context = context;
        }

        public async Task<List<EvaluationSelectItemDto>> GetEvaluationsAsync(Guid jobId)
        {
            return await _context.Evaluations
                .Where(w => w.JobId == jobId)
                .Select(s => new EvaluationSelectItemDto()
                {
                    Id = s.Id,
                    Title = s.Title
                })
                .ToListAsync();
        }

        public async Task<PaginationOutput<EvaluationDto>> GetListAsync(QueryEvaluationInput input)
        {
            var query = from a in _context.Evaluations
                        join b in _context.Jobs on a.JobId equals b.Id
                        select new EvaluationDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            JobId = a.JobId,
                            JobName = b.Title,
                            CreationTime = a.CreationTime
                        };
            if (input.JobId.HasValue)
                query = query.Where(w => w.JobId == input.JobId);

            var totalCount = await query.CountAsync();
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var evalations = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync();
            return new PaginationOutput<EvaluationDto>(totalSize, evalations);
        }

        public async Task<PaginationOutput<QuestionDto>> GetQuestionsAsync(QueryQuestionInput input)
        {
            var query = from x in _context.EvaluationQuestions
                        where x.SubjectId == input.SubjectId
                        select new QuestionDto()
                        {
                            Id = x.Id,
                            Order = x.Order,
                            Description = x.Description
                        };

            var totalCount = await query.CountAsync();
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var questions = await query.OrderBy(o => o.Order)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync();
            return new PaginationOutput<QuestionDto>(totalSize, questions);
        }

        public async Task<PaginationOutput<SubjectDto>> GetSubjectsAsync(QuerySubjectInput input)
        {
            var query = from x in _context.EvaluationSubjects
                        where x.EvaluationId == input.EvaluationId
                        select new SubjectDto()
                        {
                            Id = x.Id,
                            Weight = x.Weight,
                            Description = x.Description,
                            Keyword = x.Keyword,
                            EvaluationId = x.EvaluationId
                        };

            var totalCount = await query.CountAsync();
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var subjects = await query.OrderByDescending(o => o.Weight)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync();

            return new PaginationOutput<SubjectDto>(totalSize, subjects);
        }
    }
}
