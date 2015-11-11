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
            var collections = Client.CreateDocumentCollectionQuery(databaseLink)
                .Where(col => col.Id == _collectionId).ToArray();

            if (collections.Any())
            {
                Collection = collections.First();
            }
            else
            {
                Collection = await Client.CreateDocumentCollectionAsync(databaseLink,
                    new DocumentCollection {Id = _collectionId});
            }
        }

        private static async Task ReadOrCreateDatabase()
        {
            var query = Client.CreateDatabaseQuery()
                .Where(db => db.Id == _databaseId);

            var databases = query.ToArray();
            if (databases.Any())
            {
                _database = databases.First();
            }
            else
            {
                _database = await Client.CreateDatabaseAsync(new Database {Id = _databaseId});
            }
        }
    }
}