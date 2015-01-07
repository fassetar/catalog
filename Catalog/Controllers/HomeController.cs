using Catalog.Models;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.WebControls;

namespace Catalog.Controllers
{
	/// <summary>Main Site Controller</summary>
	[HandleError]
	public class HomeController : Controller
	{
		 /// <summary>All selectable facet fields.</summary>
		private static readonly string[] AllFacetFields = new[] { "cat", "manu_exact", "sku" };

		/// <summary>Solr service call.</summary>
		private readonly ISolrOperations<Product> _solrOperations =
			ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();

		public ActionResult Index(SearchParameters parameters)
		{
			if (null != parameters)
			{

			}
			return View();
		}

		/// <summary>Search Grid Display</summary>
		/// <param name="parameters"></param>
		/// <returns></returns>		
		public JsonResult Search(SearchParameters parameters)
		{
			try
			{
				var start = (parameters.PageIndex - 1) * parameters.PageSize;
				var matchingProducts = _solrOperations.Query(BuildQuery(parameters), new QueryOptions
				{
					FilterQueries = BuildFilterQueries(parameters),
					Rows = parameters.PageSize,
					Start = start,
					OrderBy = GetSelectedSort(parameters),
					SpellCheck = new SpellCheckingParameters(),
					Facet = new FacetParameters
					{
						Queries = AllFacetFields.Except(SelectedFacetFields(parameters))
																			  .Select(f => new SolrFacetFieldQuery(f) { MinCount = 1 })
																			  .Cast<ISolrFacetQuery>()
																			  .ToList(),
					},
				});
				ProductView view = new ProductView
				{
					Products = matchingProducts,
					Search = parameters,
					TotalCount = matchingProducts.NumFound,
					Facets = matchingProducts.FacetFields,
					DidYouMean = GetSpellCheckingResult(matchingProducts)
				};
				return Json(view, JsonRequestBehavior.AllowGet);
			}
			catch (Exception)
			{
				return Json(new ProductView
				{
					QueryError = true
				});
			}
		}

		/// <summary>Builds the Solr query from the search parameters.</summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public ISolrQuery BuildQuery(SearchParameters parameters)
		{
			if (!string.IsNullOrEmpty(parameters.FreeSearch))
				return new SolrQuery(parameters.FreeSearch);
			return SolrQuery.All;
		}

		/// <summary>Applying Search Filters</summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public ICollection<ISolrQuery> BuildFilterQueries(SearchParameters parameters)
		{
			var queriesFromFacets = from p in parameters.Facets
									select (ISolrQuery)Query.Field(p.Key).Is(p.Value);
			return queriesFromFacets.ToList();
		}


		/// <summary>Gets the selected facet fields</summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public IEnumerable<string> SelectedFacetFields(SearchParameters parameters)
		{
			return parameters.Facets.Select(f => f.Key);
		}

		/// <summary>Get current applied sorts.</summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public SortOrder[] GetSelectedSort(SearchParameters parameters)
		{
			return new[] { SortOrder.Parse(parameters.Sort) }.Where(o => o != null).ToArray();
		}

		private string GetSpellCheckingResult(SolrQueryResults<Product> products) {
			return string.Join(" ", products.SpellChecking
										.Select(c => c.Suggestions.FirstOrDefault())
										.Where(c => !string.IsNullOrEmpty(c))
										.ToArray());
		}

		[HttpGet]
		public ActionResult Bundle(string path, string type)
		{
			string filepath = Server.MapPath(path);
			string content = string.Empty;
			try
			{
				using(var stream = new StreamReader(filepath))
				{
					content = stream.ReadToEnd();
				}
			}
			catch (Exception)
			{
				return Content("<!-- WARN: null content -->");
			}
			return Content(content, type);
		}	    
	}
}