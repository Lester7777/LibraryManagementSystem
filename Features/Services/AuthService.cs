using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Features.Data.Models;
using LibraryManagement.Features.Data;

namespace LibraryManagement.Features.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Member?> RegisterAsync(string firstName, string lastName, string email, string password, string? phone = null)
        {
            email = email.Trim().ToLowerInvariant();
            if (await _db.Members.AnyAsync(m => m.Email == email))
                return null;

            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 100_000, HashAlgorithmName.SHA256, 32);

            var member = new Member
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phone,
                JoinedDate = DateTime.UtcNow,
                PasswordSalt = Convert.ToBase64String(salt),
                PasswordHash = Convert.ToBase64String(hash)
            };

            _db.Members.Add(member);
            await _db.SaveChangesAsync();
            return member;
        }

        public async Task<Member?> LoginAsync(string email, string password)
        {
            email = email.Trim().ToLowerInvariant();
            var member = await _db.Members.SingleOrDefaultAsync(m => m.Email == email);
            if (member == null)
                return null;

            var salt = Convert.FromBase64String(member.PasswordSalt);
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 100_000, HashAlgorithmName.SHA256, 32);
            var hashBase64 = Convert.ToBase64String(hash);
            if (hashBase64 != member.PasswordHash)
                return null;

            return member;
        }
    }
}
