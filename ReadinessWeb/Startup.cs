using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
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

            config.MapHttpAttributeRoutes();
        }

        private static void ConfigureStaticFiles(IAppBuilder appBuilder)
        {
            var fileSystem = new PhysicalFileSystem(@".\wwwroot");
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = fileSystem,
                RequestPath = PathString.Empty
            };

            options.DefaultFilesOptions.DefaultFileNames = new[] { "index.html" };
            options.StaticFileOptions.FileSystem = fileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.EnableDirectoryBrowsing = true;

            appBuilder.UseFileServer(options);
        }
    }
}
