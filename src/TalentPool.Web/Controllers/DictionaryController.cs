using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using TalentPool.Application;
using TalentPool.Application.Dictionaries;
using TalentPool.AspNetCore.Mvc.Authorization;
using TalentPool.AspNetCore.Mvc.Notify;
using TalentPool.Dictionaries;
using TalentPool.Web.Auth;
using TalentPool.Web.Models.CommonModels;
using TalentPool.Web.Models.DictionaryViewModels;

namespace TalentPool.Web.Controllers
{
    [AuthorizeCheck(Pages.Configuration_Dictionary)]
    public class DictionaryController : WebControllerBase
    {
        private readonly DictionaryManager _dictionaryManager;
        private readonly IDictionaryQuerier _dictionaryQuerier;

        public DictionaryController(
            IServiceProvider serviceProvider,
            DictionaryManager dictionaryManager,
             IDictionaryQuerier dictionaryQuerier)
            : base(serviceProvider)
        {
            _dictionaryManager = dictionaryManager;
            _dictionaryQuerier = dictionaryQuerier;
        }
        public async Task<IActionResult> List(PaginationInput input)
        {
            var output = await _dictionaryQuerier.GetListAsync(input);
            return View(new PaginationModel<DictionaryDto>(output, input));
        }

        public IActionResult Create()
        {
            return BuildCreateOrEditDisplay(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrEditDictionaryModel model)
        {
            if (ModelState.IsValid)
            {
                var dictionary = Mapper.Map<Dictionary>(model);

                await _dictionaryManager.CreateAsync(dictionary);

                Notifier.Success("你已成功创建了一条数据字典记录。");
                return RedirectToAction(nameof(List));
            }
            return BuildCreateOrEditDisplay(model);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            var dictionary = await _dictionaryManager.FindByIdAsync(id);
            if (dictionary == null)
                return NotFound(id);
            var model = Mapper.Map<CreateOrEditDictionaryModel>(dictionary);

            return BuildCreateOrEditDisplay(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateOrEditDictionaryModel model)
        {
            var dictionary = await _dictionaryManager.FindByIdAsync(model.Id.Value);
            if (dictionary == null)
                return NotFound(model.Id);

            if (ModelState.IsValid)
            {
                _ = Mapper.Map(model, dictionary);


                await _dictionaryManager.UpdateAsync(dictionary);

                Notifier.Success("你已成功编辑了一条数据字典记录。");
                return RedirectToAction(nameof(List));
            }
            return BuildCreateOrEditDisplay(model);
        }
        private  IActionResult  BuildCreateOrEditDisplay(CreateOrEditDictionaryModel model)
        {
            if (model == null)
            {
                model = new CreateOrEditDictionaryModel();
            }
            var dictionaries = _dictionaryManager.InjectDictionaries;
            if (dictionaries != null)
            {
                model.InjectDictionaries = dictionaries.Select(s => new SelectListItem()
                {
                    Text = s.DisplayName,
                    Value = s.Name
                }).ToList();
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {

            var dictionary = await _dictionaryManager.FindByIdAsync(id);
            if (dictionary == null)
                return NotFound(id);


            return View(new DeleteDictionaryModel() { Id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteDictionaryModel model)
        {
            var dictionary = await _dictionaryManager.FindByIdAsync(model.Id);
            if (dictionary == null)
                return NotFound(model.Id);

            await _dictionaryManager.DeleteAsync(dictionary);
            Notifier.Success("你已成功删除了一条数据字典记录。");
            return RedirectToAction(nameof(List));
        }

        private IActionResult NotFound(Guid id)
        {
            Notifier.Warning($"未找到id:“{id}”的字典记录。");
            return RedirectToAction(nameof(List));
        }
    }
}
