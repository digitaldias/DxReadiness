using Owin;
using ReadinessWeb.IoC;
using StructureMap;
using System.Web.Http;

namespace ReadinessWeb
{
    public static class Startup
    {
        public static Container DiContainer;

        public static void ConfigureApp(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureDependencyInversion();
            ConfigureStaticFiles(appBuilder);
            ConfigureRouting(config);

            appBuilder.UseWebApi(config);

        }

        private static void ConfigureDependencyInversion()
        {
            DiContainer = new Container(new RuntimeRegistry());
        }

        private static void ConfigureRouting(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void ConfigureStaticFiles(IAppBuilder appBuilder)
        {
            appBuilder.UseDefaultFiles();
            appBuilder.UseStaticFiles();
        }
    }
}
