using EntityLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {
        private static AppDbContext _instance = null!;
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
        public DbSet<Privatkund> Privatkunder { get; set; } = null!;
        public DbSet<Privatlektion> Privatlektioner { get; set; } = null!;
        public DbSet<Roll> Roller { get; set; } = null!;
        public DbSet<Skidskola> Skidskolor { get; set; } = null!;
        public DbSet<Utrustning> Utrustningar { get; set; } = null!;
        

        private static bool _test = true;
        private static bool _reset = false;

        private AppDbContext()
        {
                if (_test && !_reset)
                {
                    _reset = true;
                    base.Database.EnsureDeleted();
                }
                base.Database.EnsureCreated();
                Seed();
        }
        public static AppDbContext Instantiate()
        {
            if (_instance == null) _instance = new AppDbContext();
            return _instance;
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
                Kund Börje = new Kund(0.8f, 1, "Ekliden", "5190", "sandared", "112-1121121", "borje@mail.com", null, pkund);
                Kunder.Add(Börje);
                Företagskund fkund = new Företagskund("1111111", "Ara AB", "borki", "Hidden Leaf village", "60324", "Köping");
                Kund Ara = new Kund(0.65f, 1, "Göreborgsvägen", "51189", "borås", "911-911911", "info@ara.se", fkund, null);
                Kunder.Add(Ara);
                SaveChanges();
            }
            if (!Faciliteter.Any())
            {
                for (int i = 0; i < 85; i++)
                {
                    Facilitet facilitet;
                    Lägenhet lägenhet;
                    if (i < 50)
                    {
                        // Lägenhet 1 with 4 beds and size 50
                        lägenhet = new Lägenhet("Lägenhet 1", 4, "50m\u00b2");
                        facilitet = new Facilitet(8000, null, lägenhet, null);
                    }
                    else
                    {
                        // Lägenhet 2 with 6 beds and size 70
                        lägenhet = new Lägenhet("Lägenhet 2", 6, "70m\u00b2");
                        facilitet = new Facilitet(10000, null, lägenhet, null);
                    }
                    Faciliteter.Add(facilitet);
                }
                for (int i = 0; i < 125; i++)
                {
                    Random r = new Random();
                    Campingplats camping;
                    Facilitet facilitet;
                    if (i < 25 || (i > 75 && i <=100))
                    {
                        camping = new Campingplats("Campingplats utan el", (r.Next(4,10)*10).ToString()+"m\u00b2");
                        facilitet = new Facilitet(8000, null, null, camping);
                    }
                    else
                    {
                        camping = new Campingplats("Campingplats med el", (r.Next(9, 15)*10).ToString() + "m\u00b2");
                        facilitet = new Facilitet(8000, null, null, camping);
                    }
                    Faciliteter.Add(facilitet);
                }
                for (int i = 0; i < 8; i++)
                {
                    Konferenssal konferens;
                    Facilitet facilitet;
                    if (i < 3)
                    {
                        konferens = new Konferenssal("Konferens Stor", 50);
                        facilitet = new Facilitet(8000, konferens, null, null);
                    }
                    else
                    {
                        konferens = new Konferenssal("Konferens Liten", 20);
                        facilitet = new Facilitet(8000, konferens, null, null);
                    }
                    Faciliteter.Add(facilitet);
                }
                SaveChanges();
            }
            if (!Aktiviteter.Any())
            {
                Grupplektion grönMO = new Grupplektion("Grön", 400, 10);
                    Grupplektioner.Add(grönMO);
                Grupplektion blåMO = new Grupplektion("Blå", 415, 10);
                    Grupplektioner.Add(blåMO);
                Grupplektion rödMO = new Grupplektion("Röd", 425, 10);
                    Grupplektioner.Add(rödMO);
                Grupplektion svartMO = new Grupplektion("Svart", 455, 10);
                    Grupplektioner.Add(svartMO);
                Grupplektion grönTF = new Grupplektion("Grön", 500, 10);
                    Grupplektioner.Add(grönTF);
                Grupplektion blåTF = new Grupplektion("Blå", 515, 10);
                    Grupplektioner.Add(blåTF);
                Grupplektion rödTF = new Grupplektion("Röd", 525, 10);
                    Grupplektioner.Add(rödTF);
                Grupplektion svartTF = new Grupplektion("Svart", 555, 10);
                    Grupplektioner.Add(svartTF);
                Privatlektion priv = new Privatlektion(5, 375);
                    Privatlektioner.Add(priv);
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", priv, null, new DateTime(2023, 11, 27, 00, 00, 00), new DateTime(2023, 11, 27, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", priv, null, new DateTime(2023, 11, 28, 00, 00, 00), new DateTime(2023, 11, 28, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", priv, null, new DateTime(2023, 11, 29, 00, 00, 00), new DateTime(2023, 11, 29, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", priv, null, new DateTime(2023, 11, 30, 00, 00, 00), new DateTime(2023, 11, 30, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", priv, null, new DateTime(2023, 12, 01, 00, 00, 00), new DateTime(2023, 12, 01, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", null, grönMO, new DateTime(2023, 11, 27, 00, 00, 00), new DateTime(2023, 11, 29, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", null, blåMO, new DateTime(2023, 11, 27, 00, 00, 00), new DateTime(2023, 11, 29, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", null, rödMO, new DateTime(2023, 11, 27, 00, 00, 00), new DateTime(2023, 11, 29, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", null, svartMO, new DateTime(2023, 11, 27, 00, 00, 00), new DateTime(2023, 11, 29, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", null, grönTF, new DateTime(2023, 11, 30, 00, 00, 00), new DateTime(2023, 12, 01, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", null, blåTF, new DateTime(2023, 11, 30, 00, 00, 00), new DateTime(2023, 12, 01, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", null, rödTF, new DateTime(2023, 11, 30, 00, 00, 00), new DateTime(2023, 12, 01, 23, 59, 59)));
                Skidskolor.Add(new Skidskola(0, "Ingemar Stenmark", null, svartTF, new DateTime(2023, 11, 30, 00, 00, 00), new DateTime(2023, 12, 01, 23, 59, 59)));
                SaveChanges(); 
                foreach (Skidskola skola in Skidskolor)
                {
                    Aktiviteter.Add(new Aktivitet(skola,true));
                }
                SaveChanges();
            }
            /* FUNGERAR INTE ATT SEEDA UTRUSTNING ÄNNU!!!!
             * if (!Utrustningar.Any())
            {
                for (int i = 0; i < 176; i++)
                {
                    if (i < 6) Utrustningar.Add(new Utrustning("AS"+(i+1).ToString("000"), "Alpinskidor", 1000, "90"));
                    else if (i < 15) Utrustningar.Add(new Utrustning("AS" + (i + 1).ToString("000"), "Alpinskidor", 1000, "100"));
                    else if (i < 26) Utrustningar.Add(new Utrustning("AS" + (i + 1).ToString("000"), "Alpinskidor", 1000, "110"));
                    else if (i < 39) Utrustningar.Add(new Utrustning("AS" + (i + 1).ToString("000"), "Alpinskidor", 1000, "120"));
                    else if (i < 54) Utrustningar.Add(new Utrustning("AS" + (i + 1).ToString("000"), "Alpinskidor", 1000, "130"));
                    else if (i < 71) Utrustningar.Add(new Utrustning("AS" + (i + 1).ToString("000"), "Alpinskidor", 1000, "140"));
                    else if (i < 91) Utrustningar.Add(new Utrustning("AS" + (i + 1).ToString("000"), "Alpinskidor", 1000, "150"));
                    else if (i < 116) Utrustningar.Add(new Utrustning("AS" + (i + 1).ToString("000"), "Alpinskidor", 1000, "160"));
                    else if (i < 137) Utrustningar.Add(new Utrustning("AS" + (i + 1).ToString("000"), "Alpinskidor", 1000, "170"));
                    else if (i < 158) Utrustningar.Add(new Utrustning("AS" + (i + 1).ToString("000"), "Alpinskidor", 1000, "180"));
                    else if (i < 171) Utrustningar.Add(new Utrustning("AS" + (i + 1).ToString("000"), "Alpinskidor", 1000, "190"));
                    else Utrustningar.Add(new Utrustning("AS" + (i + 1).ToString("000"), "Alpinskidor", 1000, "200"));
                }
                SaveChanges();
                for (int i = 0; i < 250; i++)
                {
                    if (i < 6) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "25"));
                    else if (i < 14) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "26"));
                    else if (i < 20) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "27"));
                    else if (i < 30) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "28"));
                    else if (i < 39) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "29"));
                    else if (i < 49) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "30"));
                    else if (i < 60) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "31"));
                    else if (i < 70) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "32"));
                    else if (i < 79) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "33"));
                    else if (i < 90) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "34"));
                    else if (i < 104) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "35"));
                    else if (i < 119) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "36"));
                    else if (i < 143) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "37"));
                    else if (i < 167) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "38"));
                    else if (i < 181) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "39"));
                    else if (i < 200) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "40"));
                    else if (i < 215) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "41"));
                    else if (i < 226) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "42"));
                    else if (i < 236) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "43"));
                    else if (i < 242) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "44"));
                    else if (i < 247) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "45"));
                    else if (i < 249) Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "46"));
                    else Utrustningar.Add(new Utrustning("AP" + (i + 1).ToString("000"), "Alpinpjäxor", 250, "47"));
                }
                SaveChanges();
                for (int i = 0; i < 176; i++)
                {
                    if (i < 6) Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "90"));
                    else if (i < 15) Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "100"));
                    else if (i < 26) Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "110"));
                    else if (i < 39) Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "120"));
                    else if (i < 54) Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "130"));
                    else if (i < 71) Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "140"));
                    else if (i < 91) Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "150"));
                    else if (i < 116) Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "160"));
                    else if (i < 137) Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "170"));
                    else if (i < 158) Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "180"));
                    else if (i < 171) Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "190"));
                    else Utrustningar.Add(new Utrustning("LS" + (i + 1).ToString("000"), "Längdskidor", 1000, "200"));
                }
                SaveChanges();
                for (int i = 0; i < 250; i++)
                {
                    if (i < 6) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "25"));
                    else if (i < 14) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "26"));
                    else if (i < 20) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "27"));
                    else if (i < 30) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "28"));
                    else if (i < 39) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "29"));
                    else if (i < 49) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "30"));
                    else if (i < 60) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "31"));
                    else if (i < 70) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "32"));
                    else if (i < 79) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "33"));
                    else if (i < 90) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "34"));
                    else if (i < 104) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "35"));
                    else if (i < 119) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "36"));
                    else if (i < 143) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "37"));
                    else if (i < 167) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "38"));
                    else if (i < 181) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "39"));
                    else if (i < 200) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "40"));
                    else if (i < 215) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "41"));
                    else if (i < 226) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "42"));
                    else if (i < 236) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "43"));
                    else if (i < 242) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "44"));
                    else if (i < 247) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "45"));
                    else if (i < 249) Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "46"));
                    else Utrustningar.Add(new Utrustning("LP" + (i + 1).ToString("000"), "Längdpjäxor", 250, "47"));
                }
                SaveChanges();
                for (int i = 0; i < 86; i++)
                {
                    if (i < 8) Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "120"));
                    else if (i < 18) Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "130"));
                    else if (i < 27) Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "140"));
                    else if (i < 52) Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "150"));
                    else if (i < 67) Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "160"));
                    else Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "170"));
                }
                SaveChanges();
                for (int i = 0; i < 90; i++)
                {
                    if (i < 1) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "25"));
                    else if (i < 5) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "26"));
                    else if (i < 8) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "27"));
                    else if (i < 11) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "28"));
                    else if (i < 14) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "29"));
                    else if (i < 17) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "30"));
                    else if (i < 20) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "31"));
                    else if (i < 23) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "32"));
                    else if (i < 26) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "33"));
                    else if (i < 30) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "34"));
                    else if (i < 33) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "35"));
                    else if (i < 36) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "36"));
                    else if (i < 43) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "37"));
                    else if (i < 47) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "38"));
                    else if (i < 51) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "39"));
                    else if (i < 56) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "40"));
                    else if (i < 61) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "41"));
                    else if (i < 66) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "42"));
                    else if (i < 79) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "43"));
                    else if (i < 87) Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "44"));
                    else Utrustningar.Add(new Utrustning("SS" + (i + 1).ToString("000"), "Snowboardboots", 250, "45"));
                }
                SaveChanges(); 
                for (int i = 0; i < 15; i++)
                {
                    if (i < 10) Utrustningar.Add(new Utrustning("S" + (i + 1).ToString("00"), "Skoter lynx", 1000, "1 pers"));
                    else Utrustningar.Add(new Utrustning("S" + (i + 1).ToString("00"), "Skoter yamaha viking", 1000, "2 pers"));
                }
                SaveChanges();
            }*/
        }

    }
}