using System;

namespace TalentPool.AspNetCore.Mvc.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AuthorizeCheckAttribute : Attribute
    {
        public string Permission { get; }
        public AuthorizeCheckAttribute(string permission)
        {
            Permission = permission;
        }
    }
}
