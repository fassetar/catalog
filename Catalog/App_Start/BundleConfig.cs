using System.Web.Optimization;

namespace Catalog
{
	public class BundleConfig
	{				
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js",
						"~/Scripts/jquery.unobtrusive*",
						"~/Scripts/jquery.validate*"
						));

			bundles.Add(new ScriptBundle("~/bundles/angular").Include(
						"~/Scripts/angular.min.js"));			

			bundles.Add(new ScriptBundle("~/bundles/controllers").Include(
						"~/Scripts/Search.js"));

			bundles.Add(new StyleBundle("~/Content/base").Include(
				"~/Content/bootstrap.css",
				"~/Content/site.css"
				));
		}
	}
}