using League.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace League.Controllers
{
    public class ErrorsController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }


        [Route("error/403")]
        public IActionResult Error403()
        {
            return View();
        }
    }
}
