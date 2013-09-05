using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using Catalog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace Catalog.Controllers
{   
    /// <summary>A controller for Solr Query Api methods.</summary>
	public class SolrSearchController : ApiController
	{
		private readonly ISolrOperations<Product> _solrOperations =
			ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();

        /// <summary> </summary>
        /// <param name="q">Query String</param>
        /// <param name="categoryFilter">Options</param>
        /// <returns></returns>
        public ProductView Get(string q, string categoryFilter = null)
	    {
            var query = new SolrQuery(SolrQuerySanitizer.Sanitize(q));
            var queryOptions = CreateQueryOptions(categoryFilter);

            var solrResult = _solrOperations.Query(q, queryOptions);

            return CreateSearchResultFromSolrResult(solrResult);
		}

        /// <summary> </summary>
        /// <param name="solrResult"></param>
        /// <returns>Returns a SearchResult that has a items </returns>
		private static ProductView CreateSearchResultFromSolrResult(SolrQueryResults<Product> solrResult)
		{
			return new ProductView
				{

					Products = solrResult,
					TotalCount = solrResult.NumFound,
				};
		}

        /// <summary></summary>
        /// <param name="categoryFilter"></param>
        /// <returns></returns>
		private static QueryOptions CreateQueryOptions(string categoryFilter)
		{
            return new QueryOptions
                {
                    Start = 0, //<-- this might be for numbering the size of the page!?
                    //FilterQueries = CreateFilterQueries(categoryFilter/*, lowPrice, highPrice*/),
                    Facet = new FacetParameters
                    {
                       Queries = new ISolrFacetQuery[] {
                             new SolrFacetFieldQuery("cat")  }
                    }
                    //CreateFacetParameters(), //<-- here is where our FACETS come in!
                    //ExtraParams = CreateExtraParams() //<-- not sure whats the purpose.
                };
		}

        private static Dictionary<string, string> CreateExtraParams()
        {
            return new Dictionary<string, string>
            {
                { "defType", "dismax" },
                { "pf", ConfigurationManager.AppSettings["SolrPhraseFields"]}
            };
        }

        #region FACETS!
        private static FacetParameters CreateFacetParameters()
        {
            //var lessThanFifty = new SolrQueryByRange<decimal>("ListPrice", 0m, 50m);
            //var fiftyTo100 = new SolrQueryByRange<decimal>("ListPrice", 50m, 100m);
            //var oneHundredToFiveHundred = new SolrQueryByRange<decimal>("ListPrice", 100m, 500m);
            //var overFiveHundred = new SolrQueryByRange<string>("ListPrice", "500", "*");

            return new FacetParameters
            {
                Queries = new List<ISolrFacetQuery>
                {
                    new SolrFacetFieldQuery("CategoryName") { MinCount = 1 },
                    //new SolrFacetQuery(lessThanFifty),
                    //new SolrFacetQuery(fiftyTo100),
                    //new SolrFacetQuery(oneHundredToFiveHundred),
                    //new SolrFacetQuery(overFiveHundred)
                }
            };
        }
        #endregion

        //Will come back later for filtering out queries!
        private static ICollection<ISolrQuery> CreateFilterQueries(string categoryFilter)
        {
            var filterQueries = new List<ISolrQuery>();
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                filterQueries.Add(new SolrQueryByField("CategoryName", categoryFilter));
            }
            return filterQueries;
        }
    }
}
