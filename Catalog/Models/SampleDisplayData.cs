using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SolrNet;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace SolrIndexManager
{
    class SampleDisplayData
    {
        private string ConnectionString = ConfigurationManager.ConnectionStrings["Catalog Data DB"].ToString();
        private ISolrOperations<SampleDisplayData> SolrOperations;

        public SampleDisplayData()
        {

        }

        public SampleDisplayData(string solrServerURL)
        {
            SolrNet.Startup.Init<SampleDisplayData>(solrServerURL);

            this.SolrOperations = ServiceLocator.Current.GetInstance<ISolrOperations<SampleDisplayData>>();
        }
        
        #region Search & Facet Fields
        [SolrField("Repository")]
        public string Repository { get; set; }

        [SolrField("Collection")]
        public string Collection { get; set; }

        [SolrField("CatalogID")]
        public string CatalogID { get; set; }

        [SolrField("Description")]
        public string Description { get; set; }

        [SolrField("ProductName")]
        public string ProductName { get; set; }

        [SolrField("ProductType")]
        public string ProductType { get; set; }

        [SolrField("CollectionType")]
        public string CollectionType { get; set; }

        [SolrField("ClinicallyAffected")]
        public bool ClinicallyAffected { get; set; }

        [SolrField("DetailedClinicalData")]
        public bool DetailedClinicalData { get; set; }

        [SolrField("Karyotype")]
        public string Karyotype { get; set; }

        [SolrField("RelationshipToProband")]
        public string RelationshipToProband { get; set; }

        [SolrField("ReportedMutations")]
        public string ReportedMutations { get; set; }

        [SolrField("PublicationsCited")]
        public int? PublicationsCited { get; set; }

        [SolrField("FamilyNumber")]
        public string FamilyNumber { get; set; }

        [SolrField("AffectedStatus")]
        public string AffectedStatus { get; set; }
        #endregion

        #region Demographic Information

        /// <summary>
        /// This is also a search field
        /// </summary>
        [SolrField("Race")]
        public string Race { get; set; }
       
        /// <summary>
        /// This is also a search field
        /// </summary>
        [SolrField("Age")]
        public string Age { get; set; }

        /// <summary>
        /// This is also a search field
        /// </summary>
        [SolrField("Gender")]
        public string Gender { get; set; }
        
        /// <summary>
        /// This is an optional field which can be displayed in the search 
        /// </summary>
        [SolrField("Ethnicity")]
        public string Ethnicity { get; set; }

        #endregion



        public async Task<bool> IndexAllSolrData()
        {
            DataSet ds = LoadValuesFromCatalog();
            List<SampleDisplayData> dataToBeIndexed = BuildObject(ds);
            
            try
            {
                //delete all entries that exist
                SolrOperations.Delete(SolrQuery.All);

                //Now readd them
                Parallel.ForEach(dataToBeIndexed, d =>
                {
                    SolrOperations.Add(d);
                });

                SolrOperations.Commit();

                
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddSingleDocumentToSolrIndex(SampleDisplayData document)
        {
            bool success;
            try
            {
                SolrOperations.Add(document);
                SolrOperations.Commit();

                success = true;

            }
            catch
            {
                success = false;
            }

            return success;
        }

        public async Task<bool> RemoveSingleDocument(SampleDisplayData document)
        {
            bool success;

            try
            {
                SolrOperations.Delete(document);
                SolrOperations.Commit();

                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }

        #region Private Methods Used to Read Data from the DB
        private DataSet LoadValuesFromCatalog()
        {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.Catalog_GetDataForSolrIndex", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    using (DataSet ds = new DataSet())
                    {
                        adapter.SelectCommand = cmd;
                        adapter.Fill(ds);

                        return ds;
                    }
                }
        }

        private List<SampleDisplayData> BuildObject(DataSet ds)
        {
            List<SampleDisplayData> dataToBeIndexed = new List<SampleDisplayData>();
            dataToBeIndexed = (from a in ds.Tables[0].AsEnumerable()
                               select new SampleDisplayData()
                               {
                                   CatalogID = a.Field<string>("Catalog_ID"),
                                   Age = a.Field<string>("Age"),
                                   CollectionType = a.Field<string>("Collection_Type"),
                                   Description = a.Field<string>("Description"),
                                   Gender = a.Field<string>("Gender"),
                                   ProductName = a.Field<string>("Product_Type_Desc"),
                                   ProductType = a.Field<string>("Sample_Type"),
                                   Race = a.Field<string>("Race"),
                                   //unsure about where to get this field from 
                                   FamilyNumber = a.Field<string>("FamilyNumber"),
                                   AffectedStatus = a.Field<string>("Affected"),
                                   Karyotype = a.Field<string>("Karyotype"),
                                   ReportedMutations = a.Field<string>("Mutation"),
                                   PublicationsCited = a.Field<int?>("PubsSited"),
                                   //unsure about where to get this field from
                                   ClinicallyAffected = a.Field<bool>("ClinicallyAffected"),
                                   //unsure about where to get this field from 
                                   Collection = a.Field<string>("Collection"),
                                   //unsure about where to get this field from 
                                   DetailedClinicalData = a.Field<bool>("DetailedClinicalData"),
                                   Ethnicity = a.Field<string>("Ethnicity"),
                                   //unsure about where to get this field from 
                                   RelationshipToProband = a.Field<string>("RelationshipToProband"),
                                   //unsure about where to get this field from
                                   Repository = a.Field<string>("Repository")

                               }).AsParallel().ToList();

            return dataToBeIndexed;
        }
        #endregion
    }
        
}
