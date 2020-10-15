using System;
using Van.TalentPool.Investigations;

namespace Van.TalentPool.Application.Investigations
{
    public class ReportInvestigationDto
    {
        public Guid Id { get; set; }
        public Guid ResumeId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string JobName { get; set; }
        public string PlatformName { get; set; }
        public DateTime InvestigateDate { get; set; }
        public DateTime CreationTime { get; set; }
        public AcceptTravelStatus? AcceptTravelStatus { get; set; }
        public string CityOfResidence { get; set; }
        public string CityOfDomicile { get; set; }
        public string ExpectedInterviewDate { get; set; }
        public string ExpectedPhoneInterviewDate { get; set; }
        public bool? IsConnected { get; set; }
        public bool? IsQualified { get; set; }
        public string OwnerUserName { get; set; }
        public InvestigationStatus Status { get; set; } 
    }
}
