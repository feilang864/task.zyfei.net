using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using FFLTask.SRV.ViewModel.Validations;

namespace FFLTask.UI.PC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(new DataAnnotationsModelValidatorProvider());

            ResolveDependency();
            RegisterValidation();

#if DEBUG
            string file = HttpContext.Current.Server.MapPath("~/log4net.cfg.dev.xml");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(file));
#else
            string file = HttpContext.Current.Server.MapPath("~/log4net.cfg.xml");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(file));
#endif
        }

        public static IContainer container;
        void ResolveDependency()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterFilterProvider();

#if PROD
            builder.RegisterModule(new ProdServiceModule());
#endif
#if UIDEV
            builder.RegisterModule(new UIDevServicemodule());   
#endif
            container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private void RegisterValidation()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(FflRequiredAttribute),
                typeof(RequiredAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(FflStringLengthAttribute),
                typeof(StringLengthAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(FflEmailValidationAttribute),
                typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(FflBlogUrlValidateAttribute),
                typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(FflUrlValidationAttribute),
                typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(FflNumberValidationAttribute),
                typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(FflDateValidationAttribute),
                typeof(RegularExpressionAttributeAdapter));
        }
    }
}