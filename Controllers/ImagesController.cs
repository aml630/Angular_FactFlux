﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FactFluxV3.Models;
using FactFluxV3.Attribute;

namespace FactFluxV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly DB_A41BC9_aml630Context _context;

        public ImagesController(DB_A41BC9_aml630Context context)
        {
            _context = context;
        }

        // GET: api/Images
        [HttpGet]
        public IEnumerable<Images> GetImages()
        {
            return _context.Images;
        }

        // GET: api/Images/5
        [HttpGet("{contentType}/{contentId}")]
        public async Task<IActionResult> GetImages([FromRoute] string contentType, int contentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var images = await _context.Images.Where(x=>x.ContentType == contentType && x.ContentId == contentId).ToListAsync();

            if (images == null)
            {
                return NotFound();
            }

            return Ok(images);
        }

        // GET: api/Images/5
        [HttpGet("{word}")]
        public async Task<IActionResult> GetImageFromWord([FromRoute] string word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string spacedWord = word.Replace("-", " ");

            var findWord = await _context.Words.Where(x => x.Word.ToLower() == spacedWord.ToLower()).FirstOrDefaultAsync();

            var images = await _context.Images.Where(x => x.ContentType == "Word" && x.ContentId == findWord.WordId).ToListAsync();

            if (images == null)
            {
                return NotFound();
            }

            return Ok(images);
        }

        // PUT: api/Images/5
        [RoleAuth]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImages([FromRoute] int id, [FromBody] Images images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != images.ImageId)
            {
                return BadRequest();
            }

            _context.Entry(images).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagesExists(id))
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

        // POST: api/Images
        [RoleAuth]
        [HttpPost]
        public async Task<IActionResult> PostImages([FromBody] Images images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Images.Add(images);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImages", new { id = images.ImageId }, images);
        }

        // DELETE: api/Images/5
        [RoleAuth]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImages([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var images = await _context.Images.FindAsync(id);
            if (images == null)
            {
                return NotFound();
            }

            _context.Images.Remove(images);
            await _context.SaveChangesAsync();

            return Ok(images);
        }

        private bool ImagesExists(int id)
        {
            return _context.Images.Any(e => e.ImageId == id);
        }
    }
}