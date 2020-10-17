using System;

namespace Une.TalentPool.Application.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Photo { get; set; }  
        public bool Active { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
