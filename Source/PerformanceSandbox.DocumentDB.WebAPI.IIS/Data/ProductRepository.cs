using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace PerformanceSandbox.DocumentDB.WebAPI.IIS.Data
{
    public class ProductRepository : DocumentDb
    {
        //each repo can specify it's own database and document collection
        public ProductRepository() : base("SandboxDB", "Products")
        {
        }

        public Task<List<Product>> GetProductsAsync()
        {
            return Task.Run(() => Client.CreateDocumentQuery<Product>(Collection.DocumentsLink).ToList());
        }

        public Task<Product> GetProductAsync(string id)
        {
            return Task.Run(() =>
                Client.CreateDocumentQuery<Product>(Collection.DocumentsLink)
                    .Where(p => p.Id == id)
                    .AsEnumerable()
                    .FirstOrDefault());

        }

        public Task<ResourceResponse<Document>> CreateProduct(Product product)
        {
            return Client.CreateDocumentAsync(Collection.DocumentsLink, product);
        }

        public Task<ResourceResponse<Document>> UpdateProductAsync(Product product)
        {
            var doc = Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                .Where(d => d.Id == product.Id)
                .AsEnumerable() // why the heck do we need to do this??
                .FirstOrDefault();

            return Client.ReplaceDocumentAsync(doc.SelfLink, product);
        }

        public Task<ResourceResponse<Document>> DeleteProductAsync(string id)
        {
            var doc = Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)
                .Where(d => d.Id == id)
                .AsEnumerable()
                .FirstOrDefault();

            return Client.DeleteDocumentAsync(doc.SelfLink);
        }
    }
}