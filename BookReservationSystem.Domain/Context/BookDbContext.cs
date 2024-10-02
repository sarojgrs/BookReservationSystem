using Microsoft.EntityFrameworkCore;

namespace BookReservationSystem.Domain.Context
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BooksStatusHistory> BookStatusHistories { get; set; }

        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Book entity
            modelBuilder.Entity<Book>()
                .HasKey(b => b.Id); // Primary key

            modelBuilder.Entity<Book>()
                .Property(b => b.Title)
                .IsRequired(); // Title is required

            modelBuilder.Entity<Book>()
                .Property(b => b.Author)
                .IsRequired(); // Author is required


            // Configure BooksStatusHistory entity
            modelBuilder.Entity<BooksStatusHistory>()
                .HasKey(h => h.Id); // Primary key

            modelBuilder.Entity<BooksStatusHistory>()
                .HasOne<Book>()
                .WithMany()
                .HasForeignKey(h => h.BookId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
