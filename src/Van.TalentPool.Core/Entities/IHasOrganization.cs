using System;

namespace Van.TalentPool.Entities
{
    public interface IHasOrganization
    {
        Guid OrganizationId { get; set; }
    }
}
