using System.Text.RegularExpressions;

namespace Catalog
{
    /// <summary> </summary>
	public static class SolrQuerySanitizer
	{
		private static readonly Regex SolrSanitizationPattern = new Regex(
			@"\+|\-|!|\(|\)|\{|\}|\[|\]|\^|~|\*|\?|:|;|&", RegexOptions.Compiled);

        /// <summary> </summary>
		public static string Sanitize(string input)
		{
			return string.IsNullOrWhiteSpace(input) ?
				input :
				SolrSanitizationPattern.Replace(input, string.Empty);
		}
	}
}