using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Catalog
{
	public static class WebApiConfig
	{        
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{categoryFilter}",
				defaults: new { categoryFilter = RouteParameter.Optional }
			);

			config.EnableSystemDiagnosticsTracing();

			// Use camel case for JSON data.
			config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		}
	}
}
