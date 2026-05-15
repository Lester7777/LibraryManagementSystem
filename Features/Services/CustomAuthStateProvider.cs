using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace LibraryManagement.Features.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CurrentUserService _currentUserService;
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(IHttpContextAccessor httpContextAccessor, CurrentUserService currentUserService)
        {
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                var emailClaim = httpContext.User.FindFirst(ClaimTypes.Email);
                var firstNameClaim = httpContext.User.FindFirst(ClaimTypes.Name);
                var lastNameClaim = httpContext.User.FindFirst(ClaimTypes.Surname);
                var isAdminClaim = httpContext.User.FindFirst("IsAdmin");

                if (userIdClaim != null && emailClaim != null && firstNameClaim != null && lastNameClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var isAdmin = bool.TryParse(isAdminClaim?.Value, out var admin) && admin;
                    _currentUserService.SetUser(userId, emailClaim.Value, firstNameClaim.Value, lastNameClaim.Value, isAdmin);
                }

                return Task.FromResult(new AuthenticationState(httpContext.User));
            }
            else
            {
                _currentUserService.SetLoggedOut();
            }
            return Task.FromResult(new AuthenticationState(_currentUser));
        }

        public void MarkUserAsAuthenticated(ClaimsPrincipal user)
        {
            if (user.Identity != null)
            {
                _currentUser = new ClaimsPrincipal(user.Identity);
                var authState = Task.FromResult(new AuthenticationState(_currentUser));
                NotifyAuthenticationStateChanged(authState);
            }
        }

        public void MarkUserAsLoggedOut()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(_currentUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
