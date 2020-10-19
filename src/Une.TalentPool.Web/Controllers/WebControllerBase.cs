using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using Une.TalentPool.Configurations;
using Une.TalentPool.Infrastructure.Notify;

namespace Une.TalentPool.Web.Controllers
{
    [Authorize]
    public class WebControllerBase : Controller
    {
        public WebControllerBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Notifier = serviceProvider.GetRequiredService<INotifier>();
            Mapper = serviceProvider.GetRequiredService<IMapper>();
            UserIdentifier = serviceProvider.GetRequiredService<IUserIdentifier>();
            InitUserCustomSetting();
        }
        protected IServiceProvider ServiceProvider { get; }
        protected INotifier Notifier { get; }
        protected IMapper Mapper { get; }
        protected IUserIdentifier UserIdentifier { get; }
        protected UserCustomSetting CustomSetting { get; private set; }

        private async void InitUserCustomSetting()
        {
            var configurationManager = ServiceProvider.GetRequiredService<ConfigurationManager>();
            CustomSetting = await configurationManager.GetSettingAsync<UserCustomSetting>(UserIdentifier.UserId);
        }
    }
}
