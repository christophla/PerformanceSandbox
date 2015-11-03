//=============================================================================
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//=============================================================================

using System.Linq;
using CuttingEdge.Conditions;
using PerformanceSandbox.MSSQL.WebAPI.DataAccess.Domain;
using PerformanceSandbox.MSSQL.WebAPI.DataAccess.Entities;

namespace PerformanceSandbox.MSSQL.WebAPI.DataAccess.Migrations
{
    /// <summary>
    ///     Extensions for support database migrations
    /// </summary>
    internal static class MigrationExtensions
    {

        /// <summary>
        ///     Idempotent product creation
        /// </summary>
        /// <param name="context">The entity context</param>
        /// <param name="name">The product name</param>
        /// <param name="description">The product description</param>
        /// <param name="quantityAvailable">The product quantity available</param>
        public static Product TryAddProduct(this EntityContext context, string name, string description, int quantityAvailable)
        {
            Condition.Requires(context, "context").IsNotNull();

            var product = context.Products.FirstOrDefault(o => o.Name == name);
            if (null == product)
            {
                product = new Product(name) {Description = description, QuantityAvailable = quantityAvailable};
                context.Products.Add(product);
            }
            return product;
        }
    }
}