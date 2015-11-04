//=============================================================================
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//=============================================================================

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Faker;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PerformanceSandbox.MongoDB.WebApi.SelfHost.Models;

namespace PerformanceSandbox.MongoDB.WebApi.SelfHost.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly MongoClient _mongoClient;

        public ProductsController()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017");
        }

        /// <summary>
        ///     Get products
        /// </summary>
        /// <remarks>Returns a collection of all products.</remarks>
        [Route("products")]
        [AcceptVerbs("GET")]
        [ResponseType(typeof (IList<Product>))]
        public async Task<HttpResponseMessage> GetAllAsync()
        {
            var database = _mongoClient.GetDatabase("PerformanceSandbox");
            var collection = database.GetCollection<Product>("Products");

            var products = await collection.AsQueryable().ToListAsync();
            return Request.CreateResponse(products);
        }

        /// <summary>
        ///     Loads products
        /// </summary>
        /// <remarks>Loads test products in the database.</remarks>
        [Route("products/load")]
        [AcceptVerbs("POST")]
        public async Task<HttpResponseMessage> LoadAllAsync()
        {
            var database = _mongoClient.GetDatabase("PerformanceSandbox");
            var productsCollection = database.GetCollection<Product>("Products");

            var count = await productsCollection.AsQueryable().CountAsync();

            if (count <= 30)
            {
                for (var i = 0; i < 30; i++)
                {
                    var product = new Product(Company.Name())
                    {
                        Description = Lorem.Paragraph(),
                        QuantityAvailable = 10
                    };
                    await productsCollection.InsertOneAsync(product);
                }
            }

            var products = await productsCollection.AsQueryable().ToListAsync();
            return Request.CreateResponse(products);
        }

        /// <summary>
        ///     Get single product
        /// </summary>
        /// <remarks>Returns a single product</remarks>
        [Route("products/{id}")]
        [AcceptVerbs("GET")]
        [ResponseType(typeof (Product))]
        public async Task<HttpResponseMessage> GetSingleAsync(string id)
        {
            var database = _mongoClient.GetDatabase("PerformanceSandbox");
            var collection = database.GetCollection<Product>("Products");

            var product = await collection.FindAsync(o => o.Id == id);
            return Request.CreateResponse(product);
        }
    }
}