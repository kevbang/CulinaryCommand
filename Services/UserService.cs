using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CulinaryCommand.Services
{
    public interface IUserService
    {
        Task<User?> CreateUserAsync(string name, string email, string password, string role);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> ValidateCredentialsAsync(string email, string password);
    }

    public class UserService : IUserService {
        private readonly AppDbContext _context;

        // constructer
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> CreateUserAsync(string name, string email, string password, string role)
        {
            // validate if there is an existing user email
            if (await _context.Users.AnyAsync(user => user.Email == email))
            {
                return null;
            }

            // If the user doesn't already exist, create a new one:
            var user = new User
            {
                Name = name,
                Email = email,
                Password = HashPassword(password),
                Role = role,
                Phone = "",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // add user to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            return VerifyPassword(password, user.Password);
        }


        // basic sha256 hashing. we can improve in the future
        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        // verifying if the password hashes match
        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }
    }

}