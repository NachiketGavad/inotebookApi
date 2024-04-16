using inotebookApi.Data;
using inotebookApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace inotebookApi.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetNotes")]
        public async Task<IActionResult> GetNotes()
        {
            // Get the authenticated user's ID from the claims and convert it to an integer
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


            if (userId == null)
            {
                return Unauthorized();
            }

            // Fetch notes for the authenticated user
            var notes = await _context.Notes
                .Where(n => n.UserId == userId)
                .ToListAsync();

            return Ok(notes);
        }

        //[Authorize]
        [HttpPost("CreateNote")]
        public async Task<IActionResult> CreateNote(Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return Ok(note);
        }

        [Authorize]
        [HttpPut("UpdateNote/{id}")]
        public async Task<IActionResult> UpdateNote(int id, Note note)
        {
            if (id != note._id)
            {
                return BadRequest();
            }

            _context.Entry(note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
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

        [Authorize]
        [HttpDelete("DeleteNote/{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e._id == id);
        }
    }
}
