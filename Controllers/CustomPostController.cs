using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FactFluxV3.Models;

namespace FactFluxV3.Controllers
{
    public class CustomPostController : Controller
    {
        private readonly DB_A41BC9_aml630Context _context;

        public CustomPostController(DB_A41BC9_aml630Context context)
        {
            _context = context;
        }

        [Route("")]
        public async Task<IActionResult> Front()
        {
            return View();
        }


        [Route("yay")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

    }
}
