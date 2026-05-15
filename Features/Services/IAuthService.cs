using LibraryManagement.Features.Data.Models;

namespace LibraryManagement.Features.Services
{
    public interface IAuthService
    {
        Task<Member?> RegisterAsync(string firstName, string lastName, string email, string password, string? phone = null);
        Task<Member?> LoginAsync(string email, string password);
    }
}
