using Microsoft.Practices.ServiceLocation;
using SolrNet;
using System.Linq;

namespace Catalog.Helpers
{
    /// <summary>Extension used for mapping solr facets.</summary>
    public static class HtmlHelperMapperExtensions
    {
        /// <summary> </summary>
        private static IReadOnlyMappingManager mapper
        {
            get { return ServiceLocator.Current.GetInstance<IReadOnlyMappingManager>(); }
        }

        /// <summary> </summary>
        public static string SolrFieldPropName<T>(this System.Web.Mvc.HtmlHelper helper, string fieldName)
        {
            return mapper.GetFields(typeof(T)).First(p => p.Key == fieldName).Value.Property.Name;
        }

    }
}