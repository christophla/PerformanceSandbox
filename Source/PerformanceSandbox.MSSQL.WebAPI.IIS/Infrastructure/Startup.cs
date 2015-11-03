//=============================================================================
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//=============================================================================

using System.Web.Http;
using Microsoft.Owin;
using Owin;
using PerformanceSandbox.MSSQL.WebAPI.IIS.Infrastructure;
using PerformanceSandbox.MSSQL.WebAPI.IIS.Infrastructure.Config;

[assembly: OwinStartup(typeof (Startup))]

namespace PerformanceSandbox.MSSQL.WebAPI.IIS.Infrastructure
{
    /// <summary>
    ///     Application (OWIN) Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Configures the application
        /// </summary>
        /// <param name="app">The app builder</param>
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            ApiRouteConfig.Register(configuration);
            FormattersConfig.Register(configuration);

            app.UseWebApi(configuration);
        }
    }
}