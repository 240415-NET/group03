using Microsoft.EntityFrameworkCore;
using LibrarySystem.API.Models;

namespace LibrarySystem.API.Data;

public class LibrarySystemContext : DbContext
{

    public DbSet<User> Users {get; set;}
    public DbSet<Book> Books {get; set;}

    public DbSet<Checkout> Checkouts {get; set;}

    public LibrarySystemContext () {}

    public LibrarySystemContext(DbContextOptions options) : base (options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        modelBuilder.Entity<Book>()
            .ToTable("Books");
        modelBuilder.Entity<Checkout>()
            .ToTable("Checkouts");

        modelBuilder.Entity<User>()
            .ToTable("Users");

        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CS_AS");

    }   
}