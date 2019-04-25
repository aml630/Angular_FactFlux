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
        private readonly DB_A41BC9_aml630Context _context;

        public StoriesController(DB_A41BC9_aml630Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Stories> Stories(int pageNumber = 1)
        {
            int Page = pageNumber;

            int RecordsPerPage = 30;

            var storyList = _context.Words.Where(x => x.Main && x.DateIncremented > DateTime.UtcNow.AddDays(-2)).Select(x => new Stories()
            {
                WordId = x.WordId,
                Word = x.Word,
                Type = x.Type,
                Description = x.Description,
                Images = _context.Images.Where(z => z.ContentType == "Word" && z.ContentId == x.WordId).ToList(),
                Monthly = x.Monthly,
                Weekly = x.Weekly,
                DateIncremented = x.DateIncremented
            });

            var pagedList = storyList.Skip((Page - 1) * RecordsPerPage).Take(RecordsPerPage).ToList();

            return pagedList.OrderByDescending(x=>x.Weekly);
        }

        [HttpGet("GetMatching/{letters}")]
        public List<Stories> GetMatchingWords([FromRoute] string letters)
        {
            var storyList = _context.Words.Where(x => x.Word.Contains(letters)).Select(x => new Stories()
            {
                WordId = x.WordId,
                Word = x.Word,
                Type = x.Type,
                Images = _context.Images.Where(z => z.ContentType == "Word" && z.ContentId == x.WordId).ToList()
            }).Take(30).ToList();

            return storyList;
        }
    }
}