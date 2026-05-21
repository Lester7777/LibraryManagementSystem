using LibraryManagement.Features.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagement.Features.Data
{
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

            public DbSet<Book> Books { get; set; } = null!;
            public DbSet<Author> Authors { get; set; } = null!;
            public DbSet<Member> Members { get; set; } = null!;
            public DbSet<Loan> Loans { get; set; } = null!;

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                
                // Seed admin user
                var salt = RandomNumberGenerator.GetBytes(16);
                var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes("Admin123!"), salt, 100_000, HashAlgorithmName.SHA256, 32);
                
                modelBuilder.Entity<Member>().HasData(
                    new Member
                    {
                        Id = 999,
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@library.com",
                        PhoneNumber = null,
                        JoinedDate = DateTime.UtcNow,
                        PasswordHash = Convert.ToBase64String(hash),
                        PasswordSalt = Convert.ToBase64String(salt),
                        IsAdmin = true
                    }
                );

                // Seed authors
                modelBuilder.Entity<Author>().HasData(
                    new Author { Id = 1, Name = "J.K. Rowling", Biography = "British author best known for Harry Potter series" },
                    new Author { Id = 2, Name = "George Orwell", Biography = "English novelist and essayist" },
                    new Author { Id = 3, Name = "Harper Lee", Biography = "American novelist known for To Kill a Mockingbird" },
                    new Author { Id = 4, Name = "J.R.R. Tolkien", Biography = "English writer and philologist" }
                );

                // Seed books
                modelBuilder.Entity<Book>().HasData(
                    new Book { Id = 1, Title = "Harry Potter and the Philosopher's Stone", AuthorId = 1, Genre = "Fantasy", TotalCopies = 5, AvailableCopies = 5, ISBN = "978-0747532699" },
                    new Book { Id = 2, Title = "1984", AuthorId = 2, Genre = "Dystopian", TotalCopies = 3, AvailableCopies = 3, ISBN = "978-0451524935" },
                    new Book { Id = 3, Title = "To Kill a Mockingbird", AuthorId = 3, Genre = "Fiction", TotalCopies = 4, AvailableCopies = 4, ISBN = "978-0061120084" },
                    new Book { Id = 4, Title = "The Hobbit", AuthorId = 4, Genre = "Fantasy", TotalCopies = 3, AvailableCopies = 3, ISBN = "978-0547928227" }
                );
            }
        }
    }
