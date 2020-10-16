using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Van.TalentPool.Web.Models.DailyStatisticViewModels
{
    public class CreateOrEditDailyStatisticViewModel
    {
        public Guid? Id { get; set; }
        public List<SelectListItem> Platforms { get; set; }
        [Required(ErrorMessage = "请选择简历平台")]
        public string Platform { get; set; }
        public List<SelectListItem> Jobs { get; set; }
        public string Date { get; set; }
        public List<DailyStatisticItemModel> Items { get; set; }
        public string Description { get; set; }
    }

    public class DailyStatisticItemModel
    {
        public string JobName { get; set; }
        public int UpdateCount { get; set; }
        public int DownloadCount { get; set; }
    }
}
