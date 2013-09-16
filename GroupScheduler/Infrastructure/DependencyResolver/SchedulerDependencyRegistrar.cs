using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using ILog = Common.Logging.ILog;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using GroupScheduler.Infrastructure.Database.Classes;

namespace GroupScheduler.Infrastructure.DependencyResolver
{
    public class SchedulerDependencyRegistrar :DependencyRegistrar
    {
        public SchedulerDependencyRegistrar(ContainerBuilder containerBuilder) : base(containerBuilder)
        {
        }

        public override void RegisterDependencies()
        {
            this.RegisterLogger();
            RegisterSchedulerUserService();
            RegisterSchedulerContext();
            RegisterDatabaseClasses();
        }

        public void RegisterLogger()
        {
            this.ContainerBuilder
                .Register(c => LogManager.GetCurrentClassLogger())
                .SingleInstance();
        }

        public void RegisterSchedulerUserService()
        {
            this.ContainerBuilder
               .Register(c => new SchedulerUserService(c.Resolve<IAccountDb>(), new HttpContextWrapper(HttpContext.Current)))
               .As<ISchedulerUserService>()
               .InstancePerHttpRequest();
        }

        public void RegisterSchedulerContext()
        {
            this.ContainerBuilder
                .Register(c => new SchedulerContext(c.Resolve<ISchedulerUserService>()))
                .As<ISchedulerContext>()
                .InstancePerHttpRequest();
        }

        public void RegisterDatabaseClasses()
        {
            this.ContainerBuilder
                .Register(c => new AccountDb(c.Resolve<ILog>()))
                .As<IAccountDb>()
                .InstancePerHttpRequest();
            this.ContainerBuilder
                .Register(c => new GroupDb())
                .As<GroupDb>()
                .InstancePerHttpRequest();
            this.ContainerBuilder
                .Register(c => new SchedulerDb())
                .As<SchedulerDb>()
                .InstancePerHttpRequest();
        }
    }
}