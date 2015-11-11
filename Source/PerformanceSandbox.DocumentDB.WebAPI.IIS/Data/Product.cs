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
using System.Threading;
using Newtonsoft.Json;

namespace PerformanceSandbox.DocumentDB.WebAPI.IIS.Data
{
    public class Product
    {
        /// <summary>
        ///     Initializes a new product.
        /// </summary>
        /// <param name="name">The product name</param>
        public Product(string name) : this()
        {
            CreatedBy = ModifiedBy = Thread.CurrentPrincipal.Identity.Name;
            CreatedAt = ModifiedAt = DateTime.UtcNow;
            Name = name;
        }

        protected Product()
        {
        }

        /// <summary>
        ///     Indicates if the product is active
        /// </summary>
        public bool Archived { get; set; }

        /// <summary>
        ///     The date that the record was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; internal set; }

        /// <summary>
        ///     The user that originally created the record.
        /// </summary>
        public string CreatedBy { get; internal set; }

        /// <summary>
        ///     The product description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The primary key.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; internal set; }

        /// <summary>
        ///     The date that the record was last modified.
        /// </summary>
        public DateTimeOffset ModifiedAt { get; internal set; }

        /// <summary>
        ///     The user that last modified the record.
        /// </summary>
        public string ModifiedBy { get; internal set; }

        /// <summary>
        ///     The product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The product quantity available
        /// </summary>
        public int QuantityAvailable { get; set; }
    }
}