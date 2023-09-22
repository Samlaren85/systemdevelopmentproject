using EntityLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class AppDbContext : DbContext

    {
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            if (!optionBuilder.IsConfigured)
            {
                optionBuilder.UseSqlServer("Server=sqlutb2.hb.se,56077;Database=suht2301;User Id=suht2301;Password=bax999;");
                base.OnConfiguring(optionBuilder);
            }
        }

    }
}