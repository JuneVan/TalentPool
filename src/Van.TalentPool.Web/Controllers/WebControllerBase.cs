using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using Van.TalentPool.Infrastructure.Notify;

namespace Van.TalentPool.Web.Controllers
{
    [Authorize]
    public class WebControllerBase : Controller
    {
        public WebControllerBase(IServiceProvider serviceProvider)
        {
            Notifier = serviceProvider.GetRequiredService<INotifier>();
            Mapper = serviceProvider.GetRequiredService<IMapper>();
            UserIdentifier = serviceProvider.GetRequiredService<IUserIdentifier>();
        }

        public INotifier Notifier { get; }
        public IMapper Mapper { get; }
        public IUserIdentifier UserIdentifier { get; set; }
    }
}
