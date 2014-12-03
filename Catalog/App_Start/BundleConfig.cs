using System.Web.Optimization;

namespace Catalog
{
	/// <summary>Combine root styles in base and js in controllers bundle.</summary>
	public class BundleConfig
	{				
		/// <summary>Always apply minification and bundling to CUSTOM styles and javascript!</summary>
		/// <param name="bundles"></param>
		public static void RegisterBundles(BundleCollection bundles)
		{
			//TODO: tempt solution, until I think where to fit this.
			bundles.Add(new StyleBundle("~/Content/site").Include("~/Content/sites.css"));

			//Everything else should be handle in bower.
			bundles.Add(new ScriptBundle("~/bundles/controllers").Include(
						"~/Scripts/Search.js"));			
		}
	}
}