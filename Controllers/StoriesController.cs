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

namespace FactFluxV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly FactFluxV3Context _context;

        public StoriesController(FactFluxV3Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Stories> Stories()
        {
            var storyList = _context.Words.Where(x => x.Main).Select(x => new Stories() {
                WordId = x.WordId,
                Word = x.Word,
                Type = x.Type,
                Images = _context.Images.Where(z=>z.ContentType == "Word" && z.ContentId == x.WordId).ToList()
            }).ToList();

            return storyList;
        }
    }
}