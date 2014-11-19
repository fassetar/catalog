using System.Web.Mvc;

namespace Catalog
{
	/// <summary>Handle all and log all errors.</summary>
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}