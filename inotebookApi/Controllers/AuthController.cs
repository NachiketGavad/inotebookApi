using inotebookApi.Data;
using inotebookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using inotebookApi.Helpers.Utils;

namespace inotebookApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly JwtUtils _jwtUtils;

        public AuthController(ApplicationDbContext context, IConfiguration configuration, IPasswordService passwordService, JwtUtils jwtUtils)
        {
            _context = context;
            _configuration = configuration;
            _passwordService = passwordService;
            _jwtUtils = jwtUtils;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the user exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.email == user.email);
            if (existingUser != null)
            {
                return BadRequest(new { error = "User already exists" });
            }

            // Hash the password
            user.password = _passwordService.HashPassword(user.password);

            // Add the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = _jwtUtils.GenerateJwtToken(user);

            return Ok(new { success = true, auth_token = token });
        }



        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] User_login loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = ModelState });
            }

            try
            {
                // Find the user by email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.email == loginUser.email);
                if (user == null)
                {
                    return BadRequest(new { error = "Invalid email or password" });
                }

                // Compare passwords
                if (!VerifyPassword(loginUser.password, user.password))
                {
                    return BadRequest(new { error = "Invalid email or password" });
                }

                // Generate JWT token
                var token = _jwtUtils.GenerateJwtToken(user);//If login usrename and password are correct then proceed to generate token


                return Ok(new { success = true, auth_token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return _passwordService.HashPassword(inputPassword) == hashedPassword;
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
