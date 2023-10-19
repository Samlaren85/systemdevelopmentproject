using EntityLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {
        private static AppDbContext _instance = null!;
        public DbSet<Kund> Kunder { get; set; } = null!;
        public DbSet<Aktivitet> Aktiviteter { get; set; } = null!;
        public DbSet<Aktivitetsbokning> Aktivitetsbokningar { get; set; } = null!;
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
        public DbSet<Utrustningsbokning> Utrustningsbokningar { get; set; } = null!;


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
                /*
                string connectionString = new ConfigurationBuilder()
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
                SaveChanges();
            }
            if (!Användare.Any()) 
            {
                Användare.Add(new Användare("@dm1n1stratoR",1, Roller.FirstOrDefault(r => r.RollID.Equals("ROLL001"))));
                Användare.Add(new Användare("P@ssword1234", 2, Roller.FirstOrDefault(r => r.RollID.Equals("ROLL002"))));
                SaveChanges();
            }

            if (!Kunder.Any())
            {
                Privatkund pkund = new Privatkund("990106", "Börje", "Lundin");
                Kund kund = new Kund(10f, 12000, "Ekliden", "5190", "sandared", "112-1121121", "borje@mail.com", null, pkund);
                Kunder.Add(kund);

                pkund = new Privatkund("921215-5678", "Erik", "Johansson");
                kund = new Kund(0, 12000, "Lillgatan 12", "56789", "Storstaden", "070-9876543", "erik.j@example.com", null, pkund);
                Kunder.Add(kund);

                pkund = new Privatkund("890731-2345", "Maria", "Svensson");
                kund = new Kund(0, 12000, "Mellangatan 5", "34567", "Mellanstaden", "070-2345678", "maria.s@example.com", null, pkund);
                Kunder.Add(kund);

                pkund = new Privatkund("780829-8765", "Anders", "Nilsson");
                kund = new Kund(0, 12000, "Långgatan 20", "67890", "Långtuna", "070-8765432", "anders.n@example.com", null, pkund);
                Kunder.Add(kund);

                pkund = new Privatkund("950630-3456", "Sara", "Lindgren");
                kund = new Kund(0, 12000, "Kortgatan 3", "23456", "Kortbyn", "070-3456789", "sara.l@example.com", null, pkund);
                Kunder.Add(kund); 
                
                Företagskund fkund = new Företagskund("1111111", "Ara AB", "borki", "Hidden Leaf village", "60324", "Köping");
                kund = new Kund(12f, 100000, "Göreborgsvägen", "51189", "borås", "911-911911", "info@ara.se", fkund, null);
                Kunder.Add(kund);

                Företagskund fkund2 = new Företagskund("2222222", "Tech Solutions AB", "Sara Lindgren", "Innovation Road 42", "12345", "Teknoville");
                Kund kund2 = new Kund(10f, 800000, "Teknikvägen 10", "54321", "Teknostad", "555-123456", "info@techsolutions.se", fkund2, null);
                Kunder.Add(kund2);

                Företagskund fkund3 = new Företagskund("3333333", "Eco Ventures AB", "Niklas Grön", "Green Street 7", "67890", "Miljöby");
                Kund kund3 = new Kund(6f, 600000, "Återvinningsgatan 3", "98765", "Eco City", "555-987654", "info@ecoventures.se", fkund3, null);
                Kunder.Add(kund3);

                Företagskund fkund4 = new Företagskund("4444444", "Future Innovations Ltd.", "Adam Månsson", "Futuristic Lane 25", "13579", "Techtopia");
                Kund kund4 = new Kund(0f, 1200000, "Robotvägen 8", "24680", "Innovation City", "555-246810", "info@futureinnovations.com", fkund4, null);
                Kunder.Add(kund4);
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

            if (!Bokningar.Any())
            {
                Random r = new Random();
                Bokning bokning1 = new Bokning(new DateTime(2023, 10, 15), new DateTime(2023, 10, 20), Användare.FirstOrDefault(a => a.UserType == 1), Kunder.ToList()[r.Next(0, Kunder.Count() - 1)], new List<Facilitet>() { Faciliteter.ToList()[r.Next(0, Faciliteter.Count() - 1)] }, "2", true);
                Bokning bokning2 = new Bokning(new DateTime(2023, 10, 20), new DateTime(2023, 10, 22), Användare.FirstOrDefault(a => a.UserType == 1), Kunder.ToList()[r.Next(0, Kunder.Count() - 1)], new List<Facilitet>() { Faciliteter.ToList()[r.Next(0, Faciliteter.Count() - 1)] }, "6", false);
                Bokning bokning3 = new Bokning(new DateTime(2023, 10, 15), new DateTime(2023, 10, 20), Användare.FirstOrDefault(a => a.UserType == 2), Kunder.ToList()[r.Next(0, Kunder.Count() - 1)], new List<Facilitet>() { Faciliteter.ToList()[r.Next(0, Faciliteter.Count() - 1)] }, "3", true);
                Bokning bokning4 = new Bokning(new DateTime(2023, 10, 20), new DateTime(2023, 10, 22), Användare.FirstOrDefault(a => a.UserType == 1), Kunder.ToList()[r.Next(0, Kunder.Count() - 1)], new List<Facilitet>() { Faciliteter.ToList()[r.Next(0, Faciliteter.Count() - 1)] }, "2", false);
                Bokning bokning5 = new Bokning(new DateTime(2023, 11, 26), new DateTime(2023, 12, 01), Användare.FirstOrDefault(a => a.UserType == 2), Kunder.ToList()[r.Next(0, Kunder.Count() - 1)], new List<Facilitet>() { Faciliteter.ToList()[r.Next(0, Faciliteter.Count() - 1)] }, "4", true);
                Bokning bokning6 = new Bokning(new DateTime(2023, 11, 26), new DateTime(2023, 12, 03), Användare.FirstOrDefault(a => a.UserType == 2), Kunder.ToList()[r.Next(0, Kunder.Count() - 1)], new List<Facilitet>() { Faciliteter.ToList()[r.Next(0, Faciliteter.Count() - 1)] }, "2", true);
                Bokning bokning7 = new Bokning(new DateTime(2023, 11, 26), new DateTime(2023, 12, 03), Användare.FirstOrDefault(a => a.UserType == 1), Kunder.ToList()[r.Next(0, Kunder.Count() - 1)], new List<Facilitet>() { Faciliteter.ToList()[r.Next(0, Faciliteter.Count() - 1)] }, "3", false);
                Bokningar.AddRange(bokning1,bokning2,bokning3,bokning4,bokning5,bokning6,bokning7);
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
            //FUNGERAR INTE ATT SEEDA UTRUSTNING ÄNNU!!!!
            if (!Utrustningar.Any())
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
                for (int i = 0; i < 86; i++)
                {
                    if (i < 8) Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "120"));
                    else if (i < 18) Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "130"));
                    else if (i < 27) Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "140"));
                    else if (i < 52) Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "150"));
                    else if (i < 67) Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "160"));
                    else Utrustningar.Add(new Utrustning("SB" + (i + 1).ToString("000"), "Snowboard", 1000, "170"));
                }
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
                for (int i = 0; i < 15; i++)
                {
                    if (i < 10) Utrustningar.Add(new Utrustning("S" + (i + 1).ToString("00"), "Skoter lynx", 1000, "1 pers"));
                    else Utrustningar.Add(new Utrustning("S" + (i + 1).ToString("00"), "Skoter yamaha viking", 1000, "2 pers"));
                }
                SaveChanges();
            }
        }

    }
}