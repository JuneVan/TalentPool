﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Une.TalentPool.Application;
using Une.TalentPool.Application.Investigations;
using Une.TalentPool.Investigations;

namespace Une.TalentPool.Dapper.Queriers
{
    public class InvestigationQuerier : IInvestigationQuerier
    {
        private readonly TalentDbContext _context;
        public InvestigationQuerier(TalentDbContext context)
        {
            _context = context;
        }



        public async Task<PaginationOutput<InvestigationDto>> GetListAsync(QueryInvestigaionInput input)
        {
            var query = from a in _context.Investigations
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join c in _context.Jobs on b.JobId equals c.Id
                        join d in _context.Users on b.OwnerUserId equals d.Id
                        join e in _context.Users on b.CreatorUserId equals e.Id
                        select new InvestigationDto()
                        {
                            Name = a.Name,
                            Id = a.Id,
                            JobId = b.JobId,
                            JobName = c.Title,
                            ResumeId = a.ResumeId,
                            CreationTime = a.CreationTime,
                            PhoneNumber = b.PhoneNumber,
                            Status = a.Status,
                            IsQualified = a.IsQualified,
                            OwnerUserId = b.OwnerUserId,
                            OwnerUserName = d.FullName,
                            InvestigateDate = a.InvestigateDate,
                            AcceptTravelStatus = a.AcceptTravelStatus,
                            IsConnected = a.IsConnected
                        };
            if (!string.IsNullOrEmpty(input.Keyword))
                query = query.Where(w => w.Name.Contains(input.Keyword)
               || w.PhoneNumber.Contains(input.Keyword));
            if (input.JobId.HasValue)
                query = query.Where(w => w.JobId == input.JobId);
            if (input.OwnerUserId.HasValue && input.OwnerUserId != Guid.Empty)
                query = query.Where(w => w.OwnerUserId == input.OwnerUserId);
            if (input.Status.HasValue)
                query = query.Where(w => w.Status == (InvestigationStatus)input.Status.Value);
            if (input.StartTime.HasValue && input.EndTime.HasValue)
                query = query.Where(w => w.InvestigateDate >= input.StartTime.Value);


            var totalCount = await query.CountAsync();
            var totalSize = (int)Math.Ceiling(totalCount / (decimal)input.PageSize);
            var investigations = await query.OrderByDescending(o => o.CreationTime)
                 .Skip((input.PageIndex - 1) * input.PageSize)
                .Take(input.PageSize)
                 .ToListAsync();
            return new PaginationOutput<InvestigationDto>(totalSize, investigations);
        }

        public async Task<InvestigationDetailDto> GetInvestigationAsync(Guid id)
        {
            var query = from a in _context.Investigations
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join c in _context.Jobs on b.JobId equals c.Id
                        join d in _context.Users on b.OwnerUserId equals d.Id
                        join e in _context.Users on a.CreatorUserId equals e.Id
                        join f in _context.Users on a.LastModifierUserId equals f.Id into ff
                        from fff in ff.DefaultIfEmpty()
                        where a.Id == id
                        select new InvestigationDetailDto()
                        {
                            Name = a.Name,
                            Id = a.Id,
                            JobId = b.JobId,
                            JobName = c.Title,
                            ActiveDelivery = b.ActiveDelivery,
                            ResumeId = a.ResumeId,
                            PhoneNumber = b.PhoneNumber,
                            Status = a.Status,
                            IsQualified = a.IsQualified,
                            QualifiedRemark = a.QualifiedRemark,
                            OwnerUserName = d.FullName,
                            InvestigateDate = a.InvestigateDate,
                            AcceptTravelStatus = a.AcceptTravelStatus,
                            CityOfDomicile = a.CityOfDomicile,
                            CityOfResidence = a.CityOfResidence,
                            Email = b.Email,
                            Evaluation = a.Evaluation,
                            ExpectedDate = a.ExpectedDate,
                            ExpectedInterviewDate = a.ExpectedInterviewDate,
                            ExpectedPhoneInterviewDate = a.ExpectedPhoneInterviewDate,
                            ExpectedSalary = a.ExpectedSalary,
                            Information = a.Information,
                            IsAcceptInterview = a.IsAcceptInterview,
                            IsConnected = a.IsConnected,
                            CreationTime = a.CreationTime,
                            CreatorUserName = e.FullName,
                            LastModificationTime = a.LastModificationTime,
                            LastModifierUserName = fff != null ? fff.FullName : string.Empty
                        };
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<StatisticInvestigationDto>> GetStatisticInvestigationsAsync(DateTime startTime, DateTime endTime)
        {
            var query = from a in _context.Investigations
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join c in _context.Users on b.OwnerUserId equals c.Id
                        where a.CreationTime >= startTime && a.CreationTime <= endTime
                        select new StatisticInvestigationDto()
                        {
                            OwnerUserId = b.OwnerUserId,
                            OwnerUserName = c.FullName,
                            OwnerUserPhoto = c.Photo,
                            CreationTime = a.CreationTime,
                            AcceptTravelStatus = a.AcceptTravelStatus,
                            IsConnected = a.IsConnected
                        };
            return await query.ToListAsync();
        }

        public async Task<List<ReportInvestigationDto>> GetReportInvestigationsAsync(DateTime date)
        {
            var query = from a in _context.Investigations
                        join b in _context.Resumes on a.ResumeId equals b.Id
                        join c in _context.Jobs on b.JobId equals c.Id
                        join d in _context.Users on b.OwnerUserId equals d.Id
                        where a.Status != InvestigationStatus.NoStart && a.InvestigateDate == date
                        orderby a.CreationTime, a.InvestigateDate
                        select new ReportInvestigationDto()
                        {

                            Id = a.Id,
                            ResumeId = a.ResumeId,
                            Name = a.Name,
                            PhoneNumber = b.PhoneNumber,
                            JobName = c.Title,
                            PlatformName = b.PlatformName,
                            InvestigateDate = a.InvestigateDate,
                            CreationTime = a.CreationTime,
                            AcceptTravelStatus = a.AcceptTravelStatus,
                            CityOfResidence = a.CityOfResidence,
                            CityOfDomicile = a.CityOfDomicile,
                            ExpectedSalary = a.ExpectedSalary,
                            IsAcceptInterview = a.IsAcceptInterview,
                            ExpectedInterviewDate = a.ExpectedInterviewDate,
                            ExpectedPhoneInterviewDate = a.ExpectedPhoneInterviewDate,
                            IsConnected = a.IsConnected,
                            IsQualified = a.IsQualified,
                            UnconnectedRemark = a.UnconnectedRemark,
                            OwnerUserName = d.FullName,
                            Status = a.Status,
                            Information = a.Information,
                            Evaluation = a.Evaluation
                        };

            return await query.ToListAsync();
        }
    }
}
