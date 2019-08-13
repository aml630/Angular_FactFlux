using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FactFluxV3.Models;
using System.Net.Http.Headers;
using System.IO;
using FactFluxV3.Logic;
using FactFluxV3.Attribute;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using FactFluxV3.Areas.Identity.Data;

namespace FactFluxV3.Controllers
{
    [Route(".well-known/acme-challenge/XX")]
    [ApiController]
    public class CertController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Content("XX.YY", "text/plain");
        }
    }

    [Route(".well-known/acme-challenge/XX")]
    [ApiController]
    public class Cert2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Content("XX.YY", "text/plain");
        }
    }
}