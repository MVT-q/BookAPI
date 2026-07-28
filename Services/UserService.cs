using BookApi.Data;
using BookApi.DTOs;
using BookApi.Exceptions;
using BookApi.Models;
using BookApi.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookApi.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        private readonly AdminSettings _adminSettings;

        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(AppDbContext context, IOptions<AdminSettings> adminSettings, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _adminSettings = adminSettings.Value;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            return users.Select(ToDto).ToList();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            return ToDto(user);
        }

        public async Task<bool> DeleteAsync(int id, int currentUserId)
        {
            if (id == currentUserId)
                throw new CannotDeleteYourselfException("You cannot delete yourself");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if(user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeRoleAsync(int id, UserRole role)
        {
            if (!Enum.IsDefined(role))
                throw new InvalidRoleException("Invalid user role");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return false;

            user.Role = role;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task CreateAdminIfNotExistsAsync()
        {
            var adminExists = await _context.Users
                .AnyAsync(u => u.Role == UserRole.Admin);

            if (adminExists)
                return;

            var admin = new User
            {
                Username = _adminSettings.Username,
                Email = _adminSettings.Email,
                Role = UserRole.Admin
            };

            admin.PasswordHash = _passwordHasher.HashPassword(admin, _adminSettings.Password);

            _context.Users.Add(admin);

            await _context.SaveChangesAsync();
        }

        private static UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
            };
        }
    }
}
