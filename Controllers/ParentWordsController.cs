using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FactFluxV3.Models;
using FactFluxV3.Logic;
using FactFluxV3.Attribute;

namespace FactFluxV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentWordsController : ControllerBase
    {
        private readonly DB_A41BC9_aml630Context _context;

        public ParentWordsController(DB_A41BC9_aml630Context context)
        {
            _context = context;
        }

        // GET: api/ParentWords
        [HttpGet]
        public List<ParentWords> GetParentWords()
        {

            //var parentWords = _context.ParentWords.OrderBy(x => x.ParentWordId).Select(x => new ParentWithWords
            //{
            //    ParentWordId = x.ParentWordId,
            //    ParentWord = x.ParentWord,
            //    ChildWords = _context.Words.Where(z => _context.ParentWords.Where(y => y.ParentWordId == x.ParentWordId))

            //}).ToList();

            var parentWords = _context.ParentWords.OrderBy(x => x.ParentWordId).ToList();

            return parentWords;
        }

        // GET: api/ParentWords/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetParentWords([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parentWords = await _context.ParentWords.FindAsync(id);

            if (parentWords == null)
            {
                return NotFound();
            }

            return Ok(parentWords);
        }

        // PUT: api/ParentWords/5
        [RoleAuth]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParentWords([FromRoute] int id, [FromBody] ParentWords parentWords)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parentWords.WordJoinId)
            {
                return BadRequest();
            }

            _context.Entry(parentWords).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParentWordsExists(id))
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

        // POST: api/ParentWords
        [RoleAuth]
        [HttpPost]
        public async Task<IActionResult> PostParentWords([FromBody] ParentWords parentWords)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ParentWords.Add(parentWords);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParentWords", new { id = parentWords.WordJoinId }, parentWords);
        }

        // POST: api/ParentWords
        [RoleAuth]
        [HttpPost("{parentWord}/{childWord}")]
        public async Task<IActionResult> PostParentWordFromStrings([FromRoute] string parentWord, [FromRoute] string childWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wordLogic = new WordLogic();

            var foundParentWord = wordLogic.GetWordByString(parentWord);

            if (foundParentWord == null)
            {
                foundParentWord = wordLogic.CreateWord(parentWord);
            }

            var mainWordLookup = _context.Words.Where(x => x.WordId == foundParentWord.WordId).FirstOrDefault();

            mainWordLookup.Main = true;

            var foundChildWord = wordLogic.GetWordByString(childWord);

            if (foundChildWord == null)
            {
                foundChildWord = wordLogic.CreateWord(childWord);
            }

            if (mainWordLookup.Weekly < foundChildWord.Weekly)
            {
                mainWordLookup.Weekly = foundChildWord.Weekly;
            }

            if (mainWordLookup.Monthly < foundChildWord.Monthly)
            {
                mainWordLookup.Monthly = foundChildWord.Monthly;
            }

            if (mainWordLookup.Yearly < foundChildWord.Yearly)
            {
                mainWordLookup.Yearly = foundChildWord.Yearly;
            }

            var newParentWord = new ParentWords()
            {
                ParentWordId = foundParentWord.WordId,
                ChildWordId = foundChildWord.WordId
            };

            _context.ParentWords.Add(newParentWord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParentWords", new { id = newParentWord.WordJoinId }, newParentWord);
        }
        // DELETE: api/ParentWords/5
        [RoleAuth]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParentWords([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parentWords = await _context.ParentWords.FindAsync(id);
            if (parentWords == null)
            {
                return NotFound();
            }

            _context.ParentWords.Remove(parentWords);
            await _context.SaveChangesAsync();

            return Ok(parentWords);
        }

        private bool ParentWordsExists(int id)
        {
            return _context.ParentWords.Any(e => e.WordJoinId == id);
        }
    }
}