using System;

namespace Une.TalentPool.Entities
{
    public interface IHasOrganization
    {
        Guid OrganizationId { get; set; }
    }
}
