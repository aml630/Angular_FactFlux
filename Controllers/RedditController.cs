using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FactFluxV3.Models;
using Microsoft.Extensions.Configuration;
using FactFluxV3.Logic;
using FactFluxV3.Attribute;

namespace FactFluxV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedditController : ControllerBase
    {
        private readonly DB_A41BC9_aml630Context _context;
        private readonly IConfiguration Configuration;

        public RedditController(DB_A41BC9_aml630Context context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [HttpGet]
        public void Test()
        {
             
        }
    }
}