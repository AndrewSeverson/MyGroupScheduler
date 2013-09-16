using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Web;
using GroupScheduler.Infrastructure.DependencyResolver;
using IContainer = Autofac.IContainer; 

namespace GroupScheduler
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication, IContainerProviderAccessor{

        /// <summary>
        /// Holds the current Autofac container provider.
        /// </summary>
        /// <remarks>This variable is only used for ASP.NET WebForms pages. MVC does not use this at all.</remarks>
        private static IContainerProvider containerProvider;

        protected void Application_Start()
        {
            /* We use dependency injection to automagically create instances of database classes and services. These dependencies are provided to
             * the constructors of controller classes.
             */
            ContainerBuilder containerBuilder = new ContainerFactory().CreateContainerBuilder();
            IContainer container = containerBuilder.Build();

            // Setup MVC dependency injection
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Setup WebForms dependency injection
            containerProvider = new ContainerProvider(container);

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BootstrapSupport.BootstrapBundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
            BootstrapMvcSample.ExampleLayoutsRouteConfig.RegisterRoutes(RouteTable.Routes);

            
        }

        #region IContainerProviderAccessor Members

        /// <summary>
        /// Gets the current Autofac container provider.
        /// </summary>
        /// <remarks>This variable is only used for ASP.NET WebForms pages. MVC does not use this at all.</remarks>
        public IContainerProvider ContainerProvider
        {
            get { return containerProvider; }
        }

        #endregion
    }
}