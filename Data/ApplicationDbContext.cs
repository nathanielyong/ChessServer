using ChessServer.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChessServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ChessGame> ChessGames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChessGame>()
                .HasOne(c => c.WhitePlayer)
                .WithMany(u => u.WhiteChessGames)
                .HasForeignKey(c => c.WhitePlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChessGame>()
                .HasOne(c => c.BlackPlayer)
                .WithMany(u => u.BlackChessGames)
                .HasForeignKey(c => c.BlackPlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
