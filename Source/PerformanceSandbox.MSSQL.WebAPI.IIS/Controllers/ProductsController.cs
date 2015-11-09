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
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PerformanceSandbox.MSSQL.WebAPI.DataAccess.Domain;
using PerformanceSandbox.MSSQL.WebAPI.DataAccess.Entities;

namespace PerformanceSandbox.MSSQL.WebAPI.IIS.Controllers
{
    public class ProductsController : ApiController
    {
        private const string RetrieveProductRoute = "GetProductById";
        private readonly EntityContext _entityContext;

        public ProductsController()
        {
            _entityContext = new EntityContext();
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
            var products = await _entityContext.Products.OrderBy(o => o.Name).Skip(0).Take(30).ToListAsync();
            return Request.CreateResponse(products);
        }

        /// <summary>
        ///     Get a single product
        /// </summary>
        /// <remarks>Returns a single client, specified by the id parameter.</remarks>
        /// <param name="id">The id of the desired client</param>
        /// <response code="401">Credentials were not provided</response>
        /// <response code="403">Access was denied to the resource</response>
        /// <response code="404">A client was not found with given id</response>
        /// <response code="500">An unknown error occurred</response>
        [Route("products/{id:long}", Name = RetrieveProductRoute)]
        [AcceptVerbs("GET")]
        [ResponseType(typeof (Product))]
        public async Task<HttpResponseMessage> GetSingleAsync(long id)
        {
            var product = await _entityContext.Products.FindAsync(id);
            return Request.CreateResponse(product);
        }

        /// <summary>
        ///     Create a product
        /// </summary>
        /// <remarks>Creates a new product</remarks>
        /// <param name="model">The product data</param>
        /// <response code="400">Bad request</response>
        /// <response code="401">Credentials were not provided</response>
        /// <response code="403">Access was denied to the resource</response>
        /// <response code="500">An unknown error occurred</response>
        [Route("products")]
        [AcceptVerbs("POST")]
        public async Task<HttpResponseMessage> CreateAsync(Product model)
        {
            _entityContext.Products.Add(model);
            await _entityContext.SaveChangesAsync();

            var response = Request.CreateResponse(HttpStatusCode.Created);
            var uri = Url.Link(RetrieveProductRoute, new {id = model.Id});
            response.Headers.Location = new Uri(uri);
            return response;
        }
    }
}