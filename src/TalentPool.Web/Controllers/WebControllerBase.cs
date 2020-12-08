using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TalentPool.AspNetCore.Mvc.Notify;
using TalentPool.Configurations;

namespace TalentPool.Web.Controllers
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
            InitUserCustomSetting().Wait();
        }
        protected IServiceProvider ServiceProvider { get; }
        protected INotifier Notifier { get; }
        protected IMapper Mapper { get; }
        protected IUserIdentifier UserIdentifier { get; }
        protected UserCustomSetting CustomSetting { get; private set; } = new UserCustomSetting();

        private async Task InitUserCustomSetting()
        {
            var configurationManager = ServiceProvider.GetRequiredService<ConfigurationManager>();
            CustomSetting = await configurationManager.GetSettingAsync<UserCustomSetting>(UserIdentifier.UserId);
        }
    }
}
