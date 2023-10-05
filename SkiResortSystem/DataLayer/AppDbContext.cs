using EntityLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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
        public DbSet<Konferenssal> konferanssalar { get; set; } = null!;

        public DbSet<Lägenhet> Lägenheter { get; set; } = null!;
        public DbSet<Paket> Paket { get; set; } = null!;
        public DbSet<Privatkund> Privatkunder { get; set; } = null!;
        public DbSet<Privatlektion> Privatlektioner { get; set; } = null!;
        public DbSet<Roll> Roller { get; set; } = null!;
        public DbSet<Skidskola> Skidskolor { get; set; } = null!;
        public DbSet<Utrustning> Utrustningar { get; set; } = null!;
        public DbSet<Utrustningsstorlek> Utrustningsstorlekar { get; set; } = null!;


        private static bool _test = true;
        private static bool _reset = false;

        public AppDbContext()
        {
            if (_test && !_reset)
            {
                _reset = true;
                base.Database.EnsureDeleted();
            }
            base.Database.EnsureCreated();
            Seed();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            if (!optionBuilder.IsConfigured)
            {
                if (_test) optionBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SkiResortSystem;Trusted_Connection=True;");
                else optionBuilder.UseSqlServer("Server=sqlutb2.hb.se,56077;Database=suht2301;User Id=suht2301;Password=bax999;Encrypt=False;");
                /*string connectionString = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build()
                    .GetConnectionString("Test");

                optionBuilder.UseSqlServer(connectionString);*/
                base.OnConfiguring(optionBuilder);
            }
        }

        private void Seed()
        {
            if (!Roller.Any())
            {
                Roller.Add(new Roll("Admin", new List<Behörighet>()));
                Roller.Add(new Roll("Receptionist", new List<Behörighet>()));
            }
            if (!Användare.Any()) 
            {
                Användare.Add(new Användare("1",1, Roller.FirstOrDefault(r => r.RollID == "R001")));
                Användare.Add(new Användare("P@ssword1234", 2, Roller.FirstOrDefault(r => r.RollID == "R002")));
                SaveChanges();
            }
            if (!Kunder.Any())
            {
                Privatkund pkund = new Privatkund("990106", "Börje", "Lundin");
                Kund Börje = new Kund(1, 1, "Ekliden", "5190", "sandared", "112-1121121", "borje@mail.com", null, pkund);
                Kunder.Add(Börje);
                Företagskund fkund = new Företagskund("1111111", "Ara AB", "borki", "Hidden Leaf village", "60324", "Köping");
                Kund Ara = new Kund(1, 1, "Göreborgsvägen", "51189", "borås", "911-911911", "info@ara.se", fkund, null);
                Kunder.Add(Ara);
                SaveChanges();
            }
            if (!Faciliteter.Any())
            {
                Lägenhet l = new Lägenhet("Kings Bed", 10, "Typ stor");

                Facilitet Lunden = new Facilitet(100000, null, l, null);
                Faciliteter.Add(Lunden);
                SaveChanges();
            }
        }

    }
}