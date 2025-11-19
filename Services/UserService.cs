using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using CulinaryCommand.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CulinaryCommand.Services
{
    public interface IUserService
    {
        Task<User?> CreateUserAsync(string name, string email, string password, string role);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> ValidateCredentialsAsync(string email, string password);
        //Task<List<User>> GetSubordinatesAsync(User user);
    }

    public class UserService : IUserService
    {
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
                Role = Roles.Manager.ToString(),
                Phone = "",
                ManagedLocations = new List<Location>(), // list of locations
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
            return await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Locations)
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<User?> ValidateCredentialsAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            Console.WriteLine("get user by email:" + email);
            if (user == null)
            {
                return null;
            }
            // pass in input password and hashed version stored in the database
            return VerifyPassword(password, user.Password) ? user : null;
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
            // determine the hashed value of the password input by the user
            var hashOfInput = HashPassword(password);

            Console.WriteLine("INPUT HASH:     " + hashOfInput);
            Console.WriteLine("STORED HASH:    " + hashedPassword);

            return hashOfInput == hashedPassword;
        }

        // public async Task<List<User>> GetSubordinatesAsync(User user)
        // {
        //     if (user.Role == "Admin")
        //     {
        //         return await _context.Users
        //             .Where(u => u.CompanyCode == user.CompanyCode && u.Role != "Admin")
        //             .ToListAsync();
        //     }
        //     else if (user.Role == "Manager")
        //     {
        //         return await _context.Users
        //             .Where(u => u.CompanyCode == user.CompanyCode && u.Location == user.Location && u.Role == "Employee")
        //             .ToListAsync();
        //     }

        //     return new List<User>();
        // }
    }

}