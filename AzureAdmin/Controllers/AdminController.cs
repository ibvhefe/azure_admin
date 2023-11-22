// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AzureAdmin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            this._logger = logger;
        }

        [HttpGet("ExecutePowershellScript")]
        public IActionResult ExecutePowershellScript(String path)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                var result = ExecuteScript(path);

                if (!String.IsNullOrEmpty(result.Error))
                {
                    return Conflict(result.Error);
                }

                return Ok(result.Output);
            }
            catch (Exception ex)
            {
                this._logger.LogError("1", ex, "AdminController");
                return StatusCode(500);
            }
        }

        private OutputErrorPair ExecuteScript(String pathToScript)
        {
            var scriptArguments = "-ExecutionPolicy Bypass -File \"" + pathToScript + "\"";
            var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                return new OutputErrorPair { Output = output, Error = error };
            }
        }

        private class OutputErrorPair
        {
            public String Error { get; set; } = String.Empty;
            public String Output { get; set; } = String.Empty;
        }
    }
}
