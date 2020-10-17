﻿using Une.TalentPool.Application;

namespace Une.TalentPool.Web.Models.CommonModels
{
    public class PaginationModel<TDto> : PaginationOutput<TDto>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public dynamic Parameter { get; set; }

        public PaginationModel(PaginationOutput<TDto> output, dynamic parameter)
        {
            TotalSize = output.TotalSize;
            Items = output.Items;
            Parameter = parameter; 
        } 
    }
}