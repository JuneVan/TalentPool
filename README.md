# TalentPool 

企业招聘及人才库建设解决方案

## 类库

|  项目   | 目标框架  |  说明 |   依赖关系 |
|  ----  | ----  | ----|----|
| TalentPool.Core  | .NET Standard |  业务核心库，用于定义业务核心功能。包括业务模型定义、数据存储接口、业务逻辑类等、基础设施服务接口 |无|
| TalentPool.Application  | .NET Standard |  应用层类库，用于数据查询的功能。包括查询服务接口、数据传输对象（DTO）等 |TalentPool.Core|
| TalentPool.EntityFrameworkCore  | .NET Core | 基于EntityFrameworkCore的数据持久化库，包括数据存储实现类、数据模型配置、查询服务实现类等 |TalentPool.Core、TalentPool.Application|
| TalentPool.Infrastructure  | .NET Core | 基础设施层类库，包括邮件、通知、日志、异常等|TalentPool.Core、TalentPool.Application|
| TalentPool.Dapper  | .NET Core | 基于Dapper框架的数据层，主要用于SQL查询优化【暂未使用】|TalentPool.Core、TalentPool.Application|
| TalentPool.SignalR  | .NET Core | 基于SignalR框架的基础设施层，主要用于用户消息通知、用户在线状态更新【暂未使用】|TalentPool.Core、TalentPool.Application|
| TalentPool.Web  | AspNet.NET Core Web Mvc | 用户界面Web，包括控制器、视图、视图模型、前端静态资源等|TalentPool.Core、TalentPool.Application、TalentPool.EntityFrameworkCore、TalentPool.Infrastructure|

