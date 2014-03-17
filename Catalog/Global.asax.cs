using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SolrNet;
using SolrNet.Impl;
using Catalog.Models;
using Microsoft.Practices.ServiceLocation;
using System.IO;
using SolrNet.Exceptions;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Catalog.Models.Binders;
using System.Threading.Tasks;
using System.Threading;

namespace Catalog
{
    /// <summary> </summary>
	public class MvcApplication : System.Web.HttpApplication
	{
        /// <summary> </summary>
        private static readonly string solrURL = ConfigurationManager.AppSettings["solrUrl"];

        /// <summary> </summary>
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
            var solrServerUrl = ConfigurationManager.AppSettings["solrUrl"];
            var solrConnection = new SolrConnection(solrServerUrl);
            Startup.Init<Product>(solrConnection);
            ModelBinders.Binders[typeof(SearchParameters)] = new SearchParametersBinder();

            //var task = new System.Threading.Tasks.Task(AddInitialDocuments);
                AddInitialDocuments();
		}
        
        /// <summary> </summary>
        public IEnumerable<SolrNet.ExtractField> fields = new SolrNet.ExtractField[] {new ExtractField("sku","PDF") };

        /// <summary>Adds some sample documents to Solr.</summary>
        private void AddInitialDocuments()
        {
            try {
               var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
                solr.Delete(SolrQuery.All);


                var connection = ServiceLocator.Current.GetInstance<ISolrConnection>();
                foreach (var file in Directory.GetFiles(Server.MapPath("/exampledocs"), "*.xml")) {
                    connection.Post("/update", File.ReadAllText(file, Encoding.UTF8));
                }

                foreach (var file in Directory.GetFiles(Server.MapPath("/exampledocs/"), "*.pdf", SearchOption.AllDirectories))
                {
                    var fileinfo = new FileInfo(file);
                    using (FileStream fileStream = File.OpenRead(file))
                    {
                        var response = solr.Extract(
                            new ExtractParameters(fileStream, fileinfo.FullName)
                            {
                                ExtractFormat = ExtractFormat.Text,
                                ExtractOnly = false,
                                Fields = fields
                            });
                    }
                }
                solr.Commit();
                solr.BuildSpellCheckDictionary();                
            } catch (SolrConnectionException) {
                //throw new Exception(string.Format("Couldn't connect to Solr. Please make sure that Solr is running on '{0}' or change the address in your web.config, then restart the application.", solrURL));
            }
        }
	}
}