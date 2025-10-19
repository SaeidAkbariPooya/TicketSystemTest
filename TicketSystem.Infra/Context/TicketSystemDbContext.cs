using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TicketSystem.Core.Entities;
using TicketSystem.Core.IServices;

namespace TicketSystem.Infra.Context
{
    public class TicketSystemDbContext : DbContext
    {
        private readonly IPasswordHasherService _passwordHasher;
        public TicketSystemDbContext(DbContextOptions<TicketSystemDbContext> options, IPasswordHasherService passwordHasher) : base(options)
        {
            _passwordHasher = passwordHasher;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Description).IsRequired();

                entity.HasOne(t => t.CreatedByUser)
                    .WithMany(u => u.CreatedTickets)
                    .HasForeignKey(t => t.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.AssignedToUser)
                    .WithMany(u => u.AssignedTickets)
                    .HasForeignKey(t => t.AssignedToUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed initial data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    FullName = "admin",
                    Email = "admin@yahoo.com",
                    PasswordHash = _passwordHasher.HashPassword("admin123!"),
                    Role = Core.Enum.UserRole.Admin,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FullName = "user",
                    Email = "user@yahoo.com",
                    PasswordHash = _passwordHasher.HashPassword("user123!"),
                    Role = Core.Enum.UserRole.Employee,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
