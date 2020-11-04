using System;
using TalentPool.Investigations;

namespace TalentPool.Application.Investigations
{
    public  class StatisticInvestigationDto
    { 
        public DateTime CreationTime { get; set; }
        public Guid OwnerUserId { get; set; } 
        public string OwnerUserName { get; set; }
        public string OwnerUserPhoto { get; set; }
        public AcceptTravelStatus?  AcceptTravelStatus { get; set; }
        public bool? IsConnected { get; set; }
    }
}
