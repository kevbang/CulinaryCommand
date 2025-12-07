using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using CulinaryCommand.Models;

namespace CulinaryCommand.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetUsersByCompanyAsync(int companyId);
        Task UpdateUserAsync(User user);
        Task UpdateUserProfileAsync(int userId, string firstName, string lastName, string email, string phone, string role);
        Task DeleteUserAsync(int id);
        Task AssignLocationsAsync(int userId, List<int> locationIds);
        Task<List<Location>> GetLocationsForUserAsync(int userId);
        Task<List<User>> GetUsersForLocationAsync(int locationId, int? companyId = null);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> ValidateCredentialsAsync(string email, string password);
        Task<User> CreateAdminWithCompanyAndLocationAsync(FullSignupRequest req);
        Task<User> CreateInvitedUserForLocationAsync(CreateUserForLocationRequest request, int companyId, int createdByUserId);

        // Email Feat
        Task SendInviteEmailAsync(User user);
        Task<User?> GetUserByInviteTokenAsync(string token);
        Task<bool> ActivateUserAsync(string token, string password);
    }

    public class UserService : IUserService
    {
        private readonly PasswordHasher<User> _passwordHasher = new();
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;


        public UserService(AppDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // check that the user with that email DOESN'T exist
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                throw new Exception("Email already exists.");

            // <<<<<<< HEAD
            //             // If the user doesn't already exist, create a new one:
            //             var user = new User
            //             {
            //                 Name = name,
            //                 Email = email,
            //                 Password = HashPassword(password),
            //                 Role = Roles.Manager.ToString(),
            //                 Phone = "",
            //                 ManagedLocations = new List<Location>(), // list of locations
            //                 CreatedAt = DateTime.UtcNow,
            //                 UpdatedAt = DateTime.UtcNow
            //             };
            // =======
            user.Password = HashPassword(user.Password!);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            // >>>>>>> preserve/broken-signup-logic

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserLocations)
                    .ThenInclude(ul => ul.Location)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.UserLocations)
                    .ThenInclude(ul => ul.Location)
                .ToListAsync();
        }

        public async Task<List<User>> GetUsersByCompanyAsync(int companyId)
        {
            return await _context.Users
                .Where(u => u.CompanyId == companyId)
                .Include(u => u.UserLocations)
                    .ThenInclude(ul => ul.Location)
                .ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null) return;

            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.Role = user.Role;
            existing.Phone = user.Phone;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserProfileAsync(int userId, string firstName, string lastName, string email, string phone, string role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new Exception("User not found.");

            user.Name = $"{firstName} {lastName}".Trim();
            user.Email = email;
            user.Phone = phone;
            user.Role = role;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task AssignLocationsAsync(int userId, List<int> locationIds)
        {
            var existing = _context.UserLocations.Where(ul => ul.UserId == userId);
            _context.UserLocations.RemoveRange(existing);

            foreach (var locId in locationIds)
            {
                _context.UserLocations.Add(new UserLocation
                {
                    UserId = userId,
                    LocationId = locId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Location>> GetLocationsForUserAsync(int userId)
        {
            return await _context.UserLocations
                .Where(ul => ul.UserId == userId)
                .Include(ul => ul.Location)
                .Select(ul => ul.Location)
                .ToListAsync();
        }

        public async Task<List<User>> GetUsersForLocationAsync(int locationId, int? companyId = null)
        {
            var query = _context.UserLocations
                .Where(ul => ul.LocationId == locationId)
                .Include(ul => ul.User)
                .Include(ul => ul.Location)
                .AsQueryable();

            if (companyId.HasValue)
            {
                query = query.Where(ul => ul.Location.CompanyId == companyId.Value);
            }

            return await query
                .Select(ul => ul.User)
                .ToListAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Company)
                .Include(u => u.UserLocations)
                    .ThenInclude(ul => ul.Location)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> ValidateCredentialsAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null) return null;

            if (VerifyPassword(password, user.Password!))
            {
                return user;
            }
            // <<<<<<< HEAD
            // pass in input password and hashed version stored in the database
            return VerifyPassword(password, user.Password) ? user : null;
            // =======

            //             // Legacy fallback: check old SHA256 hash OR plain-text password
            //             var legacyHash = LegacySha256(password);

            //             if (string.Equals(user.Password, legacyHash, StringComparison.OrdinalIgnoreCase) ||
            //                 string.Equals(user.Password, password, StringComparison.Ordinal))
            //             {
            //                 // Migrate legacy password (SHA256 or plain text) to new Identity-style hash
            //                 user.Password = HashPassword(password);
            //                 await _context.SaveChangesAsync();
            //                 return user;
            //             }

            //             return null;
            // >>>>>>> preserve/broken-signup-logic
        }

        private string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(new User(), password);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // <<<<<<< HEAD
            // // determine the hashed value of the password input by the user
            // var hashOfInput = HashPassword(password);

            // Console.WriteLine("INPUT HASH:     " + hashOfInput);
            // Console.WriteLine("STORED HASH:    " + hashedPassword);

            // return hashOfInput == hashedPassword;
            // =======
            try
            {
                var result = _passwordHasher.VerifyHashedPassword(new User(), storedHash, password);
                return result == PasswordVerificationResult.Success;
            }
            catch
            {
                // Not a valid Identity hash → allow ValidateCredentialsAsync() to try the SHA256 fallback
                return false;
            }
            // >>>>>>> preserve/broken-signup-logic
        }

        private string LegacySha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        public async Task<User> CreateAdminWithCompanyAndLocationAsync(FullSignupRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            if (req.Admin == null) throw new Exception("Admin information is required.");
            if (req.Company == null) throw new Exception("Company information is required.");
            if (req.Location == null) throw new Exception("Location information is required.");

            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == req.Admin.Email))
                throw new Exception("A user with this email already exists.");

            // 1) Create Company
            var company = new Company
            {
                Name = req.Company.Name,
                CompanyCode = req.Company.CompanyCode,
                Address = req.Company.Address,
                City = req.Company.City,
                State = req.Company.State,
                ZipCode = req.Company.ZipCode,
                Phone = req.Company.Phone,
                Email = req.Company.Email,
                Description = req.Company.Description,
                LLCName = req.Company.LLCName,
                TaxId = req.Company.TaxId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            // 2) Create Location
            var location = new Location
            {
                Name = req.Location.Name,
                Address = req.Location.Address,
                City = req.Location.City,
                State = req.Location.State,
                ZipCode = req.Location.ZipCode,
                MarginEdgeKey = req.Location.MarginEdgeKey,
                CompanyId = company.Id
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            // 3) Create Admin User
            var admin = new User
            {
                Name = req.Admin.Name,
                Email = req.Admin.Email,
                Password = HashPassword(req.Admin.Password),
                Phone = req.Admin.Phone,
                Role = "Admin",
                CompanyId = company.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(admin);
            await _context.SaveChangesAsync();

            // 4) Link Admin to Location (UserLocation table)
            var link = new UserLocation
            {
                UserId = admin.Id,
                LocationId = location.Id
            };

            _context.UserLocations.Add(link);
            await _context.SaveChangesAsync();

            return admin;
        }

        public async Task<User> CreateInvitedUserForLocationAsync(CreateUserForLocationRequest request, int companyId, int createdByUserId)
        {
            // normalize email
            var email = request.Email.Trim().ToLowerInvariant();

            // prevent duplicate email within same company
            var exists = await _context.Users
                .AnyAsync(u => u.Email == email && u.CompanyId == companyId);

            if (exists)
            {
                throw new InvalidOperationException("A user with this email already exists for this company.");
            }

            // generate invite token now; email comes later (step C)
            var inviteToken = Guid.NewGuid().ToString("N");

            var user = new User
            {
                Name = request.FirstName.Trim() + " " + request.LastName.Trim(),
                Email = email,
                Role = request.Role,              // "Manager" or "Employee"
                CompanyId = companyId,

                // assuming these fields exist / you can add them:
                IsActive = false,            // not active until they set password
                EmailConfirmed = false,
                InviteToken = inviteToken,
                InviteTokenExpires = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = createdByUserId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // link to location as a user
            var userLocation = new UserLocation
            {
                UserId = user.Id,
                LocationId = request.LocationId
            };
            _context.UserLocations.Add(userLocation);

            // if they're a manager, also add ManagerLocation link
            if (string.Equals(request.Role, "Manager", StringComparison.OrdinalIgnoreCase))
            {
                var managerLocation = new ManagerLocation
                {
                    UserId = user.Id,
                    LocationId = request.LocationId
                };
                _context.ManagerLocations.Add(managerLocation);
            }

            await _context.SaveChangesAsync();

            // we'll send the invite email in step C using user.InviteToken
            return user;
        }

        public async Task SendInviteEmailAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.InviteToken))
                throw new InvalidOperationException("User does not have an invite token.");

            string link = $"https://yourdomain.com/account/setup?token={user.InviteToken}";

            string subject = "Your CulinaryCommand Account Invitation";
            string body = $@"
                <h2>Welcome to CulinaryCommand!</h2>
                <p>You have been invited to join <strong>{user.Company?.Name}</strong>.</p>
                <p>Click the button below to set your password and activate your account:</p>
                <p><a href='{link}' style='padding:10px 20px;background:#4CAF50;color:white;text-decoration:none;border-radius:4px;'>Set Your Password</a></p>
                <p>If the button doesn't work, use this link:</p>
                <p>{link}</p>
            ";

            // implement actual email send (SendGrid, SMTP, Mailgun, whatever)
            await _emailSender.SendEmailAsync(user.Email!, subject, body);
        }

        public async Task<User?> GetUserByInviteTokenAsync(string token)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.InviteToken == token &&
                                        u.InviteTokenExpires > DateTime.UtcNow &&
                                        u.IsActive == false);
        }

        public async Task<bool> ActivateUserAsync(string token, string password)
        {
            var user = await GetUserByInviteTokenAsync(token);

            if (user == null)
                return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            user.IsActive = true;
            user.EmailConfirmed = true;
            user.InviteToken = null;
            user.InviteTokenExpires = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}