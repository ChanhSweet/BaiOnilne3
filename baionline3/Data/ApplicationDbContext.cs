using baionline3.Models;
using Microsoft.EntityFrameworkCore;

namespace baionline3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Intern> Interns { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AllowAccess> AllowAccesses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasMany(r => r.AllowAccesses)
                .WithOne(a => a.Role)
                .HasForeignKey(a => a.RoleId);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.User)
                .WithOne(u => u.Role)
                .HasForeignKey<User>(u => u.RoleId);
        }
    }
}
