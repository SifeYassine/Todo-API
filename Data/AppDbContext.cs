using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Todo> Todos { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<AccessToken> AccessTokens { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Todo>()
    .HasOne(todo => todo.User)
    .WithMany(user => user.Todos)
    .HasForeignKey(todo => todo.UserId)
    .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<User>()
    .HasIndex(user => user.Email)
    .IsUnique();

    modelBuilder.Entity<User>()
    .HasIndex(user => user.Username)
    .IsUnique();

    modelBuilder.Entity<AccessToken>()
    .HasIndex(token => token.TokenHash)
    .IsUnique();
            
    modelBuilder.Entity<AccessToken>()
    .HasOne(token => token.User)
    .WithMany()
    .HasForeignKey(token => token.UserId)
    .OnDelete(DeleteBehavior.Cascade);
  }
}