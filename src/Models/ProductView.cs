using System.Collections.Generic;

namespace Catalog.Models
{
    public class ProductView
    {

        public SearchParameters Search { get; set; }
        public ICollection<Product> Products { get; set; }
        public int TotalCount { get; set; }
        public IDictionary<string, ICollection<KeyValuePair<string, int>>> Facets { get; set; }
        public string DidYouMean { get; set; }
        public bool QueryError { get; set; }
        public string ErrorString { get; set; }

        public ProductView()
        {
            Search = new SearchParameters();
            Facets = new Dictionary<string, ICollection<KeyValuePair<string, int>>>();
            Products = new List<Product>();
        }
    }
}