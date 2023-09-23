using EntityLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class AppDbContext : DbContext

    {
        public DbSet<User> Users { get; set; } = null!;

        public AppDbContext()
        {
            base.Database.EnsureCreated();
            Seed();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            if (!optionBuilder.IsConfigured)
            {
                optionBuilder.UseSqlServer("Server=sqlutb2.hb.se,56077;Database=suht2301;User Id=suht2301;Password=bax999;Encrypt=False");
                base.OnConfiguring(optionBuilder);
            }
        }

        private void Seed()
        {
            if (!Users.Any()) 
            {
                Users.Add(new User("Admin", 1));
                Users.Add(new User("Password", 2));
                SaveChanges();
            }
        }

    }
}