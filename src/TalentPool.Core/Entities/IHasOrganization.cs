using System;

namespace TalentPool.Entities
{
    public interface IHasOrganization
    {
        Guid OrganizationId { get; set; }
    }
}
