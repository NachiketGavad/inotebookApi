using inotebookApi.Data;
using inotebookApi.Helpers.Utils;
using inotebookApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using inotebookApi.Helpers.Utils;

namespace inotebookApi.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtUtils _jwtUtils;

        public NotesController(ApplicationDbContext context,JwtUtils jwtutils)
        {
            _context = context;
            _jwtUtils = jwtutils;
        }


        [HttpGet("GetNotes")]
        public async Task<IActionResult> GetNotes()
        {
            // Get the authenticated user's ID from the claims and convert it to an integer


            // Get the JWT token from the request headers
            var token = HttpContext.Request.Headers["auth_token"].FirstOrDefault()?.Split(" ").Last();

            //var userId = 10;

            // Check if the token is missing or invalid
            if (token == null)
            {
                return StatusCode(500, new { success = false, error = "Authorization token missing" });
            }

            try
            {
                // Validate and decode the JWT token
                var claimsPrincipal = _jwtUtils.ValidateJwtToken(token); // Implement this method to validate and decode the JWT token

                // Extract the user's ID from the claims
                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                {
                    return StatusCode(500, new { success = false, error = "Invalid or missing user ID claim" });
                }

                // Fetch notes for the authenticated user
                var notes = await _context.Notes
                    .Where(n => n.UserId == userId)
                    .ToListAsync();

                return Ok(new { success = true, notes = notes });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        [HttpPost("CreateNote")]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest request)
        {

            // Get the JWT token from the request headers
            var token = HttpContext.Request.Headers["auth_token"].FirstOrDefault()?.Split(" ").Last();

            //var userId = 10;

            // Check if the token is missing or invalid
            if (token == null)
            {
                return StatusCode(500, new { success = false, error = "Authorization token missing" });
            }

            try
            {
                // Validate and decode the JWT token
                var claimsPrincipal = _jwtUtils.ValidateJwtToken(token); // Implement this method to validate and decode the JWT token

                // Extract the user's ID from the claims
                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                {
                    return StatusCode(500, new { success = false, error = "Invalid or missing user ID claim" });
                }
                // Create a new Note entity from the request data
                var note = new Note
            {
                title = request.Title,
                description = request.Description,
                tag = request.Tag,
                // Assign the user ID based on the authenticated user
                UserId = userId
            };

            // Add the note to the database
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            // Return a 201 Created response with the created note
            return CreatedAtAction(nameof(GetNotes), new { id = note._id }, note);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }


        [HttpPut("UpdateNote/{id}")]
        //[Authorize] // Ensure only authenticated users can update notes
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

        [HttpDelete("DeleteNote/{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteNote(int id)
        {
            // Get the JWT token from the request headers
            var token = HttpContext.Request.Headers["auth_token"].FirstOrDefault()?.Split(" ").Last();

            //var userId = 10;

            // Check if the token is missing or invalid
            if (token == null)
            {
                return StatusCode(500, new { success = false, error = "Authorization token missing" });
            }

            try
            {
                // Validate and decode the JWT token
                var claimsPrincipal = _jwtUtils.ValidateJwtToken(token); // Implement this method to validate and decode the JWT token

                // Extract the user's ID from the claims
                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                {
                    return StatusCode(500, new { success = false, error = "Invalid or missing user ID claim" });
                }

                var note = await _context.Notes.FindAsync(id);
                if (note == null)
                {
                    return NotFound();
                }

                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e._id == id);
        }
    }
}
