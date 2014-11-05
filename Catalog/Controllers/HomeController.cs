using Catalog.Models;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Catalog.Controllers
{
    /// <summary> </summary>
    [HandleError]
	public class HomeController : Controller
	{
         /// <summary>All selectable facet fields.</summary>
        private static readonly string[] AllFacetFields = new[] { "cat", "manu_exact", "sku" };

        /// <summary>Solr service call.</summary>
        private readonly ISolrOperations<Product> _solrOperations =
            ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();

        /// <summary>Home page for the Coriell Catalog.</summary>
        public ActionResult Index(SearchParameters parameters)
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
                    //DidYouMean = GetSpellCheckingResult(matchingProducts),
                };
                return View(view);
            }
            catch (/*InvalidFieldException*/Exception)
            {
                return View(new ProductView
                {
                    QueryError = true,
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

        /// <summary> </summary>
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
    }
}