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
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly DB_A41BC9_aml630Context _context;

        public WordsController(DB_A41BC9_aml630Context context)
        {
            _context = context;
        }

        // GET: api/Words
        [RoleAuth]
        [HttpGet]
        public IEnumerable<Words> GetWords()
        {
            return _context.Words.Where
                (x=>x.DateIncremented > DateTime.UtcNow.AddDays(-2) && (x.Word.Length > 4|| !x.Word.Any(z => char.IsUpper(z)))) 
                .Take(500).OrderByDescending(x => x.Yearly);
        }

        [HttpGet("GetMatching/{letters}")]
        public List<Words> GetMatchingWords([FromRoute] string letters)
        {

            var wordList = _context.Words.Where(x => x.Word.Contains(letters)).ToList();

            return wordList;
        }

        [HttpGet("GetMain")]
        public IEnumerable<Words> GetMainWords()
        {
            var wordList = _context.Words.Where(x => x.Main == true);

            return wordList;
        }

        // GET: api/Words/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWords([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var words = await _context.Words.FindAsync(id);

            if (words == null)
            {
                return NotFound();
            }

            return Ok(words);
        }

        // PUT: api/Words/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWords([FromRoute] int id, [FromBody] Words words)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != words.WordId)
            {
                return BadRequest();
            }

            _context.Entry(words).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WordsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Words
        [HttpPost]
        public async Task<IActionResult> PostWords([FromBody] Words words)
        {
            words.DateCreated = DateTime.UtcNow;

            words.DateIncremented = DateTime.UtcNow;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Words.Add(words);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWords", new { id = words.WordId }, words);
        }

        [HttpPost("AddImage/{contentType}/{contentId}")]
        public async Task<IActionResult> PostImageToWord([FromRoute] string contentType, int contentId, string hotLink = null)
        {
            var findImage = _context.Images.Where(x => x.ContentId == contentId && x.ContentType == contentType).FirstOrDefault();

            if (!string.IsNullOrEmpty(hotLink))
            {
                CreateImageFromHotlink(contentType, contentId, hotLink, findImage);
            }
            else
            {
                var filesUploaded = HttpContext.Request.Form.Files;

                if (filesUploaded != null)
                {
                    var imageLogic = new ImageLogic();

                    imageLogic.CreateImage(contentType, contentId, filesUploaded, findImage);
                }
            }
            return Ok();
        }

        private void CreateImageFromHotlink(string contentType, int contentId, string hotLink, Images findImage)
        {
            if (findImage==null)
            {
                var newImage = new Images()
                {
                    ContentType = contentType,
                    ContentId = contentId,
                    ImageLocation = hotLink
                };

                _context.Images.Add(newImage);

                _context.SaveChanges();
            }
            else
            {
                findImage.ImageLocation = hotLink;
                _context.SaveChanges();
            }
        }

        // DELETE: api/Words/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWords([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var words = await _context.Words.FindAsync(id);
            if (words == null)
            {
                return NotFound();
            }

            _context.Words.Remove(words);
            await _context.SaveChangesAsync();

            return Ok(words);
        }

        private bool WordsExists(int id)
        {
            return _context.Words.Any(e => e.WordId == id);
        }
    }
}