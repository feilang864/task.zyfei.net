#if PROD

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ProdService;

namespace FFLTask.UI.PC
{
    public class ProdServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisterService>().As<IRegisterService>();
            builder.RegisterType<TaskService>().As<ITaskService>();
            builder.RegisterType<ProjectConfigService>().As<IProjectConfigService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<ProjectService>().As<IProjectService>();
            builder.RegisterType<AuthorizationService>().As<IAuthroizationService>();
            builder.RegisterType<MessageService>().As<IMessageService>();
            builder.RegisterType<TeamService>().As<ITeamService>();

            base.Load(builder);
        }
    }
}

#endif
