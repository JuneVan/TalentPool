using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalentPool.Application.Evaluations;
using TalentPool.Application.Jobs;
using TalentPool.AspNetCore.Mvc.Authorization;
using TalentPool.AspNetCore.Mvc.Notify;
using TalentPool.Evaluations;
using TalentPool.Investigations;
using TalentPool.Web.Auth;
using TalentPool.Web.Models.CommonModels;
using TalentPool.Web.Models.EvaluationViewModels;

namespace TalentPool.Web.Controllers
{
    [AuthorizeCheck(Pages.Evaluation)]
    public class EvaluationController : WebControllerBase
    {
        private readonly IEvaluationQuerier _evaluationQuerier;
        private readonly IJobQuerier _jobQuerier;
        private readonly EvaluationManager _evaluationManager;
        private readonly InvestigationManager _investigationManager;
        public EvaluationController(IEvaluationQuerier evaluationQuerier,
            IJobQuerier jobQuerier,
            EvaluationManager evaluationManager,
            InvestigationManager investigationManager,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _evaluationQuerier = evaluationQuerier;
            _jobQuerier = jobQuerier;
            _evaluationManager = evaluationManager;
            _investigationManager = investigationManager;
        }
        #region Evaluation
        public async Task<IActionResult> List(QueryEvaluationInput input)
        {
            var output = await _evaluationQuerier.GetListAsync(input);
            var model = new QueryEvaluationViewModel()
            {
                Output = new PaginationModel<EvaluationDto>(output, input)
            };
            return await BuildListDisplayAsync(model);
        }
        private async Task<IActionResult> BuildListDisplayAsync(QueryEvaluationViewModel model)
        {
            var jobs = await _jobQuerier.GetJobsAsync();
            if (jobs != null)
            {
                model.Jobs = jobs.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.Title
                }).ToList();
            }

            return View(model);
        }

        // 创建
        public async Task<IActionResult> Create()
        {
            return await BuildCreateOrEditDisplayAsync(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrEditEvaluationViewModel model)
        {

            if (ModelState.IsValid)
            {
                var evaluation = Mapper.Map<Evaluation>(model);
                await _evaluationManager.CreateAsync(evaluation);
                Notifier.Success("你已成功创建了一条新的技术评测记录！");
                return RedirectToAction(nameof(List));
            }
            return await BuildCreateOrEditDisplayAsync(model);
        }


        // 编辑
        public async Task<IActionResult> Edit(Guid id)
        {
            var evaluation = await _evaluationManager.FindByIdAsync(id);
            if (evaluation == null)
                return NotFound(id);

            return await BuildCreateOrEditDisplayAsync(Mapper.Map<CreateOrEditEvaluationViewModel>(evaluation));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateOrEditEvaluationViewModel model)
        {

            if (ModelState.IsValid)
            {
                var evaluation = await _evaluationManager.FindByIdAsync(model.Id.Value);
                if (evaluation == null)
                    return NotFound(model.Id);
                _ = Mapper.Map(model, evaluation);
                await _evaluationManager.UpdateAsync(evaluation);
                Notifier.Information("你已成功编辑了一条技术评测记录！");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }

        private async Task<IActionResult> BuildCreateOrEditDisplayAsync(CreateOrEditEvaluationViewModel model)
        {
            if (model == null)
                model = new CreateOrEditEvaluationViewModel();
            var jobs = await _jobQuerier.GetJobsAsync();
            if (jobs != null)
            {
                model.Jobs = jobs.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.Title
                }).ToList();
            }
            return View(model);
        }
        // 删除
        public async Task<IActionResult> Delete(Guid id)
        {

            var evaluation = await _evaluationManager.FindByIdAsync(id);
            if (evaluation == null)
                return NotFound(id);

            return View(Mapper.Map<DeleteEvaluationViewModel>(evaluation));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteEvaluationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var evaluation = await _evaluationManager.FindByIdAsync(model.Id);
                if (evaluation == null)
                    return NotFound(model.Id);
                await _evaluationManager.DeleteAsync(evaluation);
                Notifier.Success("你已成功删除了一条技术评测记录！");
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }
        // 查看详情
        public async Task<IActionResult> View(Guid id)
        {

            var evaluation = await _evaluationManager.FindByIdAsync(id);

            if (evaluation == null)
                return NotFound(id);

            var model = Mapper.Map<EvaluationViewModel>(evaluation);

            if (model.Subjects != null)
            {
                // 由权重排序
                model.Subjects = model.Subjects.OrderByDescending(o => o.Weight).ToList();
                foreach (var subject in model.Subjects)
                {
                    var subjectQuestions = evaluation.Questions.Where(w => w.SubjectId == subject.Id).OrderBy(o => o.Order).ToList();
                    subject.Questions = Mapper.Map<List<QuestionViewModel>>(subjectQuestions);
                }
            }

            return View(model);
        }
        #endregion

        #region Subject
        public async Task<IActionResult> Subjects(QuerySubjectInput input)
        {
            var output = await _evaluationQuerier.GetSubjectsAsync(input);
            return View(new PaginationModel<SubjectDto>(output, input));

        }
        public IActionResult CreateSubject(Guid evaluationId)
        {

            return View(new CreateOrEditSubjectViewModel()
            {
                EvaluationId = evaluationId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubject(CreateOrEditSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var subject = Mapper.Map<EvaluationSubject>(model);
                await _evaluationManager.CreateSubjectAsync(subject);
                Notifier.Success("你已成功创建了一条新的技术点记录！");
                return RedirectToAction(nameof(Subjects), new { model.EvaluationId });
            }
            return View(model);
        }
        public async Task<IActionResult> EditSubject(Guid id)
        {
            var subject = await _evaluationManager.FindSubjectByIdAsync(id);
            if (subject == null)
                return NotFoundSubject(id);
            var model = Mapper.Map<CreateOrEditSubjectViewModel>(subject);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubject(CreateOrEditSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var subject = await _evaluationManager.FindSubjectByIdAsync(model.Id.Value);
                if (subject == null)
                    return NotFoundSubject(model.Id.Value);
                subject = Mapper.Map(model, subject);
                await _evaluationManager.UpdateSubjectAsync(subject);
                Notifier.Success("你已成功编辑了一条技术点记录！");
                return RedirectToAction(nameof(Subjects), new { model.EvaluationId });
            }
            return View(model);

        }
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            var subject = await _evaluationManager.FindSubjectByIdAsync(id);
            if (subject == null)
                return NotFoundSubject(id);
            var moddel = Mapper.Map<DeleteSubjectViewModel>(subject);
            return View(moddel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubject(DeleteSubjectViewModel model)
        {
            var subject = await _evaluationManager.FindSubjectByIdAsync(model.Id);
            if (subject == null)
                return NotFoundSubject(model.Id);
            await _evaluationManager.DeleteSubjectAsync(subject);
            Notifier.Success("你已成功删除了一条技术点记录！");
            return RedirectToAction(nameof(Subjects), new { model.EvaluationId });
        }
        #endregion

        #region Question
        public async Task<IActionResult> Questions(QueryQuestionInput input)
        {
            var output = await _evaluationQuerier.GetQuestionsAsync(input);
            return View(new PaginationModel<QuestionDto>(output, input));
        }

        public IActionResult CreateQuestion(Guid evaluationId, Guid subjectId)
        {
            return View(new CreateOrEditQuestionViewModel()
            {
                EvaluationId = evaluationId,
                SubjectId = subjectId
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(CreateOrEditQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = Mapper.Map<EvaluationQuestion>(model);
                await _evaluationManager.CreateQuestionAsync(question);
                Notifier.Success("你已成功创建了一条新的技术评测问题记录！");
                return RedirectToAction(nameof(Questions), new { model.EvaluationId, model.SubjectId });
            }
            return View(model);
        }

        public async Task<IActionResult> EditQuestion(Guid id)
        {
            var question = await _evaluationManager.FindQuestionByIdAsync(id);
            if (question == null)
                return NotFoundQuestion(id);
            var model = Mapper.Map<CreateOrEditQuestionViewModel>(question);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(CreateOrEditQuestionViewModel model)
        {

            if (ModelState.IsValid)
            {
                var question = await _evaluationManager.FindQuestionByIdAsync(model.Id.Value);
                if (question == null)
                    return NotFoundQuestion(model.Id.Value);
                _ = Mapper.Map(model, question);
                await _evaluationManager.UpdateQuestionAsync(question);
                Notifier.Success("你已成功编辑了一条技术评测问题记录！");
                return RedirectToAction(nameof(Questions), new { model.EvaluationId, model.SubjectId });
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var question = await _evaluationManager.FindQuestionByIdAsync(id);
            if (question == null)
                return NotFoundQuestion(id);

            var model = Mapper.Map<DeleteQuestionViewModel>(question);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestion(DeleteQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = await _evaluationManager.FindQuestionByIdAsync(model.Id);
                if (question == null)
                    return NotFoundQuestion(model.Id);
                await _evaluationManager.DeleteQuestionAsync(question);
                Notifier.Success("你已成功删除了一条技术问题记录！");
                return RedirectToAction(nameof(Questions), new { model.EvaluationId, model.SubjectId });
            }
            return View(model);
        }
        #endregion



        #region 评估
        public async Task<IActionResult> SelectEvaluation(Guid investigationId, Guid jobId)
        {
            var investigation = await _investigationManager.FindByIdAsync(investigationId);
            if (investigation == null)
                return NotFound(investigationId);

            var evaluations = await _evaluationQuerier.GetEvaluationsAsync(jobId);

            var model = new SelectEvaluationViewModel()
            {
                InvestigationId = investigationId,
                Name = investigation.Name
            };
            model.Evaluations = evaluations
                .Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.Title
                }).ToList();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectEvaluation(SelectEvaluationViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Evaluate), new { model.EvaluationId, model.InvestigationId, model.Name });
            }
            return View(model);
        }
        public async Task<IActionResult> Evaluate(Guid investigationId, Guid evaluationId)
        {
            var evaluation = await _evaluationManager.FindByIdAsync(evaluationId);
            if (evaluation == null)
                return NotFound();

            var model = Mapper.Map<EvaluationViewModel>(evaluation);

            if (evaluation.Subjects != null)
            {
                // 由权重排序
                model.Subjects = model.Subjects.OrderByDescending(o => o.Weight).ToList();
                foreach (var subject in model.Subjects)
                {
                    var subjectQuestions = evaluation.Questions.Where(w => w.SubjectId == subject.Id).OrderBy(o => o.Order).ToList();
                    subject.Questions = Mapper.Map<List<QuestionViewModel>>(subjectQuestions);
                }
            }

            return View(new EvaluateViewModel()
            {
                InvestigationId = investigationId,
                Evaluation = model
            });
        }

       
        #endregion




        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:{id}的技术评测记录。");
            return RedirectToAction(nameof(List));
        } 
        private IActionResult NotFoundSubject(Guid id)
        {
            Notifier.Warning($"未找到id:{id}的技术点记录记录。");
            return RedirectToAction(nameof(List));
        }
        private IActionResult NotFoundQuestion(Guid id)
        {
            Notifier.Warning($"未找到id:{id}的技术问题记录记录。");
            return RedirectToAction(nameof(List));
        } 
        private IActionResult NotFoundInvestigation(Guid id)
        {
            Notifier.Warning($"未找到id:{id}的意向调查记录。");
            return RedirectToAction(nameof(List));
        }
    }
}
