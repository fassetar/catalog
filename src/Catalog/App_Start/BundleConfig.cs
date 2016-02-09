using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Catalog
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/main").Include(
                "~/Scripts/angular.js",                
                "~/Scripts/ui-bootstrap-tpls.js",
                "~/Scripts/ui-grid.js",
                "~/Scripts/search.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome.css",
                "~/Content/ui-grid.css",
                "~/Content/site.css"));            
        }
    }
}