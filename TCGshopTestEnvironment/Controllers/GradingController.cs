using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Services;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Controllers
{
    public class GradingController : Controller
    {

        public IActionResult Index() 
        
        {
            return View();
        }

    }

}