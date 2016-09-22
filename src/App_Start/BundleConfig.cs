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
                "~/Scripts/search.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css"));
        }
    }
}