namespace Une.TalentPool.Application.Roles
{
    public class RoleDto: Dto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool Protected { get; set; }
    }
}
