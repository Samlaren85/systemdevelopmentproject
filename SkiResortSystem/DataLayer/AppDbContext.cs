using EntityLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<Kund> Kunder { get; set; } = null!;
        public DbSet<Aktivitet> Aktiviteter { get; set; } = null!;
        public DbSet<Användare> Användare { get; set; } = null!;
        public DbSet<Behörighet> Behörigheter { get; set; } = null!;
        public DbSet<Bokning> Bokningar { get; set; } = null!;
        public DbSet<Campingplats> Campingplatser { get; set; } = null!;
        public DbSet<Facilitet> Faciliteter { get; set; } = null!;
        public DbSet<Faktura> Fakturor { get; set; } = null!;
        public DbSet<Företagskund> Företagskunder { get; set; } = null!;
        public DbSet<Grupplektion> Grupplektioner { get; set; } = null!;
        public DbSet<Konferanssal> konferanssalar { get; set; } = null!;

        public DbSet<Lägenhet> Lägenheter { get; set; } = null!;
        public DbSet<Paket> Paket { get; set; } = null!;
        public DbSet<Privatkund> Privatkunder { get; set; } = null!;
        public DbSet<Privatlektion> Privatlektioner { get; set; } = null!;
        public DbSet<Roll> Roller { get; set; } = null!;
        public DbSet<Skidskola> Skidskolor { get; set; } = null!;
        public DbSet<Utrustning> Utrustningar { get; set; } = null!;
        public DbSet<Utrustningsstorlek> Utrustningsstorlekar { get; set; } = null!;




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