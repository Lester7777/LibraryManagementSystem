using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Features.Data.Models
{
    public class Author
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Biography { get; set; }
        
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }

    public class Book
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? ISBN { get; set; }
        
        [Required]
        public int AuthorId { get; set; }
        
        public Author Author { get; set; } = null!;
        
        [StringLength(50)]
        public string? Genre { get; set; }
        
        public int TotalCopies { get; set; }
        
        public int AvailableCopies { get; set; }
        
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }

    public class Member
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        public string? PhoneNumber { get; set; }
        
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string PasswordSalt { get; set; } = string.Empty;

        public bool IsAdmin { get; set; } = false;

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }

    public class Loan
    {
        public int Id { get; set; }
        
        [Required]
        public int BookId { get; set; }
        
        public Book Book { get; set; } = null!;
        
        [Required]
        public int MemberId { get; set; }
        
        public Member Member { get; set; } = null!;
        
        public DateTime LoanDate { get; set; } = DateTime.UtcNow;
        
        public DateTime DueDate { get; set; }
        
        public DateTime? ReturnDate { get; set; }
        
        public bool IsReturned => ReturnDate.HasValue;
    }
}
