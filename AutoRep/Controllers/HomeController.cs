using AutoRep.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Diagnostics;

namespace AutoRep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleMananger;

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleMananger)
        {
            _logger = logger;
            _roleMananger = roleMananger;

            if(!_roleMananger.RoleExistsAsync("mananger").Result)
                _roleMananger.CreateAsync(new IdentityRole ("mananger"));
            if (!_roleMananger.RoleExistsAsync("master").Result)
                _roleMananger.CreateAsync(new IdentityRole("master"));
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}