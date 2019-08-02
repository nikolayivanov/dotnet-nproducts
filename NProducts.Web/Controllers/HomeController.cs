using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NProducts.Web.Models;

namespace NProducts.Web.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exhandlerpath = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ErrorTimeStamp = DateTime.Now.Ticks.ToString();
            this.logger.LogError(exhandlerpath.Error, "An Error occured [{errortimestamp}] message [{errormsg}].", ErrorTimeStamp, exhandlerpath.Error.Message);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorPath = exhandlerpath.Path, ErrorTimeStamp = ErrorTimeStamp });
        }
    }
}
