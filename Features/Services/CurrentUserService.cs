namespace LibraryManagement.Features.Services
{
    public class CurrentUserService
    {
        public bool IsLoggedIn { get; private set; }
        public int? Id { get; private set; }
        public string? Email { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public bool IsAdmin { get; private set; }
        public string? FullName => FirstName != null && LastName != null ? $"{FirstName} {LastName}" : FirstName ?? LastName;

        public void SetUser(int id, string email, string firstName, string lastName, bool isAdmin)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            IsAdmin = isAdmin;
            IsLoggedIn = true;
        }

        public void SetLoggedOut()
        {
            Id = null;
            Email = null;
            FirstName = null;
            LastName = null;
            IsAdmin = false;
            IsLoggedIn = false;
        }
    }
}
