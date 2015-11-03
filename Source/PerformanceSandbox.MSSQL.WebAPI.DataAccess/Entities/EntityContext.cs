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
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using NLog;
using PerformanceSandbox.Common;
using PerformanceSandbox.MSSQL.WebAPI.DataAccess.Domain;
using PerformanceSandbox.MSSQL.WebAPI.DataAccess.Entities.Conventions;
using PerformanceSandbox.MSSQL.WebAPI.DataAccess.Entities.Mappings;

namespace PerformanceSandbox.MSSQL.WebAPI.DataAccess.Entities
{
    /// <summary>
    ///     Entity database context
    /// </summary>
    public class EntityContext : DbContext
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public EntityContext() : base(Constants.DataAccess.ConnectionStringName)
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;

            ObjectContext.SavingChanges += UpdateAuditable;
        }

        public virtual DbSet<Product> Products { get; set; }

        internal ObjectContext ObjectContext
        {
            get
            {
                try
                {
                    return ((IObjectContextAdapter) this).ObjectContext;
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex.Message);
                    throw new Exception("Unable to initialize the database.", ex);
                }
            }
        }

        /// <summary>
        ///     Adds mappings to the context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // conventions
            modelBuilder.Conventions.Add(new DateTime2Convention());

            // entities
            modelBuilder.Configurations.Add(new ProductMapping());
        }

        private static void UpdateAuditable(object sender, EventArgs e)
        {
            var context = sender as ObjectContext;
            if (context == null)
                return;

            // Created 

            foreach (var entry in context.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Where(entry => entry.Entity is EntityBase))
            {
                ((EntityBase) entry.Entity).CreatedAt = ((EntityBase) entry.Entity).ModifiedAt = DateTime.UtcNow;
                ((EntityBase) entry.Entity).CreatedBy = ((EntityBase) entry.Entity).ModifiedBy = Thread.CurrentPrincipal.Identity.Name;
            }

            // Updated

            foreach (var entry in context.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).Where(entry => entry.Entity is EntityBase))
            {
                ((EntityBase) entry.Entity).ModifiedAt = DateTime.UtcNow;
                ((EntityBase) entry.Entity).ModifiedBy = Thread.CurrentPrincipal.Identity.Name;
            }
        }
    }
}