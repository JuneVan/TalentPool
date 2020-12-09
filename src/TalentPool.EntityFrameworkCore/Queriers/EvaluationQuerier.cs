using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentPool.Application;
using TalentPool.Application.Evaluations;

namespace TalentPool.EntityFrameworkCore.Queriers
{
    public class EvaluationQuerier : IEvaluationQuerier
    {
        private readonly TalentDbContext _context;
        private readonly ISignal _signal;
        public EvaluationQuerier(TalentDbContext context,
             ISignal signal)
        {
            _context = context;
            _signal = signal;
        }
        protected CancellationToken CancellationToken => _signal.Token;

        public async Task<List<EvaluationSelectItemDto>> GetEvaluationsAsync(Guid jobId)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (jobId == null)
                throw new ArgumentNullException(nameof(jobId));

            return await _context.Evaluations
                .Where(w => w.JobId == jobId)
                .Select(s => new EvaluationSelectItemDto()
                {
                    Id = s.Id,
                    Title = s.Title
                })
                .ToListAsync(CancellationToken);
        }

        public async Task<PaginationOutput<EvaluationDto>> GetListAsync(QueryEvaluationInput input)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (input == null)
                throw new ArgumentNullException(nameof(input));

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

            var totalCount = await query.CountAsync(CancellationToken);
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var evalations = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync(CancellationToken);
            return new PaginationOutput<EvaluationDto>(totalSize, evalations);
        }

        public async Task<PaginationOutput<QuestionDto>> GetQuestionsAsync(QueryQuestionInput input)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var query = from x in _context.EvaluationQuestions
                        where x.SubjectId == input.SubjectId
                        select new QuestionDto()
                        {
                            Id = x.Id,
                            Order = x.Order,
                            Description = x.Description
                        };

            var totalCount = await query.CountAsync(CancellationToken);
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var questions = await query.OrderBy(o => o.Order)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync(CancellationToken);
            return new PaginationOutput<QuestionDto>(totalSize, questions);
        }

        public async Task<PaginationOutput<SubjectDto>> GetSubjectsAsync(QuerySubjectInput input)
        {
            CancellationToken.ThrowIfCancellationRequested();
            if (input == null)
                throw new ArgumentNullException(nameof(input));

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

            var totalCount = await query.CountAsync(CancellationToken);
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var subjects = await query.OrderByDescending(o => o.Weight)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync(CancellationToken);

            return new PaginationOutput<SubjectDto>(totalSize, subjects);
        }
    }
}
