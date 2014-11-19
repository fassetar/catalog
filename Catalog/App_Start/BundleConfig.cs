using System.Web.Optimization;

namespace Catalog
{
	/// <summary>Combine root styles in base and js in controllers bundle.</summary>
	public class BundleConfig
	{				
		/// <summary>Always apply minification and bundling!</summary>
		/// <param name="bundles"></param>
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
							"~/Scripts/jquery-{version}.min.js",
							"~/Scripts/jquery.unobtrusive.min.js",
							"~/Scripts/jquery.validate.min.js"
						));

			bundles.Add(new ScriptBundle("~/bundles/angular").Include(
							"~/Scripts/angular.min.js", 
							"~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
							"~/Scripts/angular-ui/ui-bootstrap.min.js"
					   ));

			bundles.Add(new ScriptBundle("~/bundles/controllers").Include(
						"~/Scripts/Search.js"));

			bundles.Add(new StyleBundle("~/Content/base").Include(
							"~/Content/bootstrap.css",
							"~/Content/site.css"
						));
		}
	}
}