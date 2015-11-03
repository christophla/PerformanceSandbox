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
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Faker;
using NLog;
using PerformanceSandbox.MSSQL.WebAPI.DataAccess.Entities;
using PerformanceSandbox.MSSQL.WebAPI.DataAccess.Migrations;

namespace AngularSkeleton.DataAccess.Migrations
{
    /// <summary>
    ///     Configures the database.
    /// </summary>
    /// <remarks>
    ///     Seeds data depending on the build mode (Debug or Release).
    /// </remarks>
    internal sealed class Configuration : DbMigrationsConfiguration<EntityContext>
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EntityContext context)
        {
            try
            {
                // sql auditing
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("MIGRATION"), null);

                // Create 30 products

                if (context.Products.Count() > 30)
                    return;

                for (var i = 0; i < 30; i++)
                {
                    context.TryAddProduct(Company.Name(), Lorem.Sentence(), RandomNumber.Next(0, 1000));
                }

                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var message = new StringBuilder();

                foreach (var eve in ex.EntityValidationErrors)
                {
                    message.AppendFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State, ex);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        message.AppendLine().AppendFormat("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }

                _logger.Error(message.ToString());
                throw new Exception(message.ToString());
            }
        }
    }
}