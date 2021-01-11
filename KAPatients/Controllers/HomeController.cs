using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KAPatients.Models;
using Microsoft.AspNetCore.Authorization;

namespace KAPatients.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        //Returns the view for index
        public IActionResult Index()
        {
         
            return View();
        }
        //Returns the view for privacy
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //action to show the error message
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
