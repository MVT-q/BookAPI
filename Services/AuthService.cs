using BookApi.Data;
using BookApi.DTOs;
using BookApi.Exceptions;
using BookApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookApi.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        private readonly AppDbContext _context;

        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null) 
                return null;

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key: key,
                algorithm: SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            
            string tokenString = tokenHandler.WriteToken(token);

            return new LoginResponseDto 
            {
                Token = tokenString,
            };
        }

        public async Task RegisterAsync(RegisterDto request)
        {
            bool exists = await _context.Users.AnyAsync(u => u.Username == request.Username);

            if (exists)
                throw new UserAlreadyExistsException("Username already exists");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Role = UserRole.User
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }
    }
}
