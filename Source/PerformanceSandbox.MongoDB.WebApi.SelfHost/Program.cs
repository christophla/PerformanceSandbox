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
using Microsoft.Owin.Hosting;
using PerformanceSandbox.MongoDB.WebApi.SelfHost.Infrastructure;

namespace PerformanceSandbox.MongoDB.WebApi.SelfHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var baseAddress = "http://localhost:7904/";

            // Start OWIN host 
            using (var app = WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("API running on: " + baseAddress);
                Console.WriteLine("Press any key to stop...");
                Console.ReadLine();
            }
        }
    }
}