using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace PerformanceSandbox.DocumentDB.WebAPI.IIS.Data
{
    public class DocumentDb
    {
        private static DocumentClient _client;
        private static string _collectionId;
        private static Database _database;
        private static string _databaseId;

        public DocumentDb(string database, string collection)
        {
            _databaseId = database;
            _collectionId = collection;
            ReadOrCreateDatabase().Wait();
            ReadOrCreateCollection(_database.SelfLink).Wait();
        }

        protected static DocumentClient Client
        {
            get
            {
                if (_client == null)
                {
                    var endpoint = ConfigurationManager.AppSettings["endpoint"];
                    var authKey = ConfigurationManager.AppSettings["authKey"];
                    var endpointUri = new Uri(endpoint);
                    _client = new DocumentClient(endpointUri, authKey);
                }
                return _client;
            }
        }

        protected static DocumentCollection Collection { get; private set; }

        private static async Task ReadOrCreateCollection(string databaseLink)
        {
            Collection = Client.CreateDocumentCollectionQuery(databaseLink).Where(col => col.Id == _collectionId).AsEnumerable().FirstOrDefault();

            if (Collection == null)
            {
                var collectionSpec = new DocumentCollection {Id = _collectionId};
                var requestOptions = new RequestOptions {OfferType = "S1"};

                Collection = await Client.CreateDocumentCollectionAsync(databaseLink, collectionSpec, requestOptions);
            }
        }

        private static async Task ReadOrCreateDatabase()
        {
            _database = Client.CreateDatabaseQuery().Where(o => o.Id == _databaseId).AsEnumerable().FirstOrDefault();

            if (_database == null)
            {
                _database = await Client.CreateDatabaseAsync(new Database {Id = _databaseId});
            }
        }
    }
}