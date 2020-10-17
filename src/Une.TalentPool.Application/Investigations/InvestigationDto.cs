using System;
using Une.TalentPool.Investigations;

namespace Une.TalentPool.Application.Investigations
{
    public class InvestigationDto
    {
        public Guid Id { get; set; }
        public Guid ResumeId { get; set; }
        public string Name { get; set; }
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public DateTime InvestigateDate { get; set; }
        public DateTime CreationTime { get; set; } 
        public InvestigationStatus Status { get; set; }
        public AcceptTravelStatus? AcceptTravelStatus { get; set; }
        public bool? IsQualified { get; set; }
        public bool? IsConnected { get; set; }
    }
}
