//=============================================================================
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//=============================================================================

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Faker;
using PerformanceSandbox.DocumentDB.WebAPI.IIS.Data;

namespace PerformanceSandbox.DocumentDB.WebAPI.IIS.Controllers
{
    public class ProductsController : ApiController
    {
        private const string RetrieveProductRoute = "GetProductById";
        private readonly ProductRepository _productRepository;

        public ProductsController()
        {
            _productRepository = new ProductRepository();
        }

        /// <summary>
        ///     Get products
        /// </summary>
        [Route("products")]
        [AcceptVerbs("GET")]
        [ResponseType(typeof (IList<Product>))]
        public async Task<HttpResponseMessage> GetAllAsync()
        {
            var products = await _productRepository.GetProductsAsync();
            return Request.CreateResponse(products);
        }

        /// <summary>
        ///     Get a single product
        /// </summary>
        [Route("products/{id}", Name = RetrieveProductRoute)]
        [AcceptVerbs("GET")]
        [ResponseType(typeof (Product))]
        public async Task<HttpResponseMessage> GetSingleAsync(string id)
        {
            var product = await _productRepository.GetProductAsync(id);
            return Request.CreateResponse(product);
        }

        /// <summary>
        ///     Loads products
        /// </summary>
        [Route("products/load")]
        [AcceptVerbs("POST")]
        public async Task<HttpResponseMessage> LoadAsync(Product model)
        {
            var existing = await _productRepository.GetProductsAsync();
            if (existing.Count < 30)
            {
                for (var i = 0; i < 30; i++)
                {
                    var product = new Product(Company.Name())
                    {
                        Archived = false,
                        Description = Lorem.Sentence(),
                        QuantityAvailable = RandomNumber.Next(1, 1000)
                    };
                    await _productRepository.CreateProduct(product);
                }
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        ///     Create a product
        /// </summary>
        [Route("products")]
        [AcceptVerbs("POST")]
        public async Task<HttpResponseMessage> CreateAsync(Product model)
        {
            await _productRepository.CreateProduct(model);
            var response = Request.CreateResponse(HttpStatusCode.Created);
            var uri = Url.Link(RetrieveProductRoute, new {id = model.Id});
            response.Headers.Location = new Uri(uri);
            return response;
        }
    }
}