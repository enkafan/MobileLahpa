using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using LahpaMobile.Services;
using LahpaMobile.Web.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;

namespace LahpaMobile.Web.App_Start
{
    public static class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? Go to: http://bit.ly/YE8OJj.
            var container = new Container();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcAttributeFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            container.Register<IWebPageScheduleParserService, WebPageScheduleParserService>();
            container.Register<IGameParser, GameParser>();
            container.Register<IScraperService, ScraperService>();
            container.Register<IScheduleParser, ScheduleParser>();
            container.Register<IScheduleService, ScheduleService>();
            container.Register<HttpContext>(() => HttpContext.Current);
            container.RegisterPerWebRequest<ICachedScheduleService, CachedScheduleService>();
        }
    }
}