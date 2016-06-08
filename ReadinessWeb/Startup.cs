using System.Web.Http;
using Owin;
using StructureMap;
using ReadinessWeb.IoC;

namespace ReadinessWeb
{
    public static class Startup
    {
        public static Container DiContainer;

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public static void ConfigureApp(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            DiContainer = new Container(new RuntimeRegistry());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);            
        }
    }
}
