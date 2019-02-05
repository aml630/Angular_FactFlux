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
    public class WordsController : ControllerBase
    {
        private readonly FactFluxV3Context _context;

        public WordsController(FactFluxV3Context context)
        {
            _context = context;
        }

        // GET: api/Words
        [HttpGet]
        public IEnumerable<Words> GetWords()
        {
            return _context.Words.Take(500).OrderByDescending(x => x.Yearly);
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
        public async Task<IActionResult> PostImageToWord([FromRoute] string contentType, int contentId)
        {
            var newFileName = string.Empty;

            if (HttpContext.Request.Form.Files != null)
            {
                var fileName = string.Empty;

                var files = HttpContext.Request.Form.Files;

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //Getting FileName
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting file Extension
                        var FileExtension = Path.GetExtension(fileName);

                        string startupPath = Environment.CurrentDirectory;

                        // concating  FileName + FileExtension
                        newFileName = myUniqueFileName + FileExtension;

                        // Combines two strings into a path.
                        fileName = Path.Combine(startupPath, "ClientApp\\src\\assets\\images") + $@"\{newFileName}";

                        var savePath = "/assets/images/" + newFileName;

                        using (FileStream fs = System.IO.File.Create(fileName))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }

                        var newImage = new Images()
                        {
                            ContentType = contentType,
                            ContentId = contentId,
                            ImageLocation = savePath
                        };

                        using (var db = new FactFluxV3Context())
                        {
                            db.Images.Add(newImage);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return Ok();
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