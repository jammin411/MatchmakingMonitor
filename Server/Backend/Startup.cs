using System;
using System.IO;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Backend
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var config = new HttpConfiguration();
			var routes = RouteTable.Routes;

			config.MapHttpAttributeRoutes(new CentralizedPrefixProvider("api"));
			routes.Clear();


#if DEBUG
      config.EnableSwagger(c =>
      {
        c.PrettyPrint();
        c.SingleApiVersion("v1", "MatchmakingMonitor");
        c.DescribeAllEnumsAsStrings();
      }).EnableSwaggerUi(c =>
      {
        c.DisableValidator();
      });
#endif

			config.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

			ConfigureStaticFiles(app);

			app.UseWebApi(config);
		}

		private static void ConfigureStaticFiles(IAppBuilder app)
		{
			var root = AppDomain.CurrentDomain.BaseDirectory;
			var wwwroot = Path.Combine(root, "wwwroot");

			var fileServerOptions = new FileServerOptions
			{
				EnableDefaultFiles = true,
				EnableDirectoryBrowsing = false,
				RequestPath = new PathString(string.Empty),
				FileSystem = new PhysicalFileSystem(wwwroot)
			};

			fileServerOptions.StaticFileOptions.ServeUnknownFileTypes = true;
			app.UseFileServer(fileServerOptions);
		}
	}
}