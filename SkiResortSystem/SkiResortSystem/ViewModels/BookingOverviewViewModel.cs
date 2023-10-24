using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.DirectoryServices;
using DynamicPDFCoreSuite.Examples;
using BusinessLayer.PrintController;

namespace SkiResortSystem.ViewModels
{
    class BookingOverviewViewModel : ObservableObject
    {
        private string kundPresentation;
        public string KundPresentation
        {
            get { return kundPresentation; }
            set { kundPresentation = value; OnPropertyChanged(); }
        }
        private Kund kund;
        public Kund Kund
        {
            get { return kund; }
            set { 
                kund = value;
                kundPresentation = kund.ToString().Split(" (")[0];
                OnPropertyChanged(); }
        }

        private bool uppdateraBokning = false;
        public bool UppdateraBokning
        {
            get { return uppdateraBokning; }
            set { uppdateraBokning = value; OnPropertyChanged(); }
        }
        private int antalNätter;
        public int AntalNätter
        {
            get { return antalNätter; }
            set { antalNätter = value; OnPropertyChanged(); }
        }
        private int antalTimmar;
        public int AntalTimmar
        {
            get { return antalTimmar; }
            set { antalTimmar = value; OnPropertyChanged(); }
        }

        private float utnyttjadKredit;
        public float UtnyttjadKredit
        {
            get { return utnyttjadKredit; }
            set { utnyttjadKredit = value; OnPropertyChanged(); }
        }

        private string facilitetstyp;
        public string Facilitetstyp
        {
            get { return facilitetstyp; }
            set { facilitetstyp = value; OnPropertyChanged(); }
        }

        private string bokningsnummer;
        public string Bokningsnummer
        {
            get { return bokningsnummer; }
            set { bokningsnummer = value; OnPropertyChanged(); }
        }

        private Bokning? bokning;
        public Bokning? Bokning
        {
            get { return bokning; }
            set { bokning = value; OnPropertyChanged(); }
        }

        private bool avbetalningsskydd;
        public bool Avbetalningsskydd
        {
            get { return avbetalningsskydd; }
            set { avbetalningsskydd = value; OnPropertyChanged(); }
        }


        private string antalPersoner;
        public string AntalPersoner
        {
            get { return antalPersoner; }
            set { antalPersoner = value; OnPropertyChanged(); }
        }

        private bool antalPersonerReadOnly = true;
        public bool AntalPersonerReadOnly
        {
            get { return antalPersonerReadOnly; }
            set
            {
                antalPersonerReadOnly = value;
                OnPropertyChanged();
            }
        }

        private float varavMoms;
        public float VaravMoms
        {
            get { return varavMoms; }
            set { varavMoms = value; OnPropertyChanged(); }
        }

        private float totalpris;
        public float Totalpris
        {
            get { return totalpris; }
            set { totalpris = value; OnPropertyChanged(); }
        }

        private DateTime ankomst;
        public DateTime Ankomst
        {
            get { return ankomst; }
            set {
                ankomst = value;
                if (ankomst > avresa) Avresa = ankomst;
                OnPropertyChanged();
            }
        }
        private Visibility visaTid = Visibility.Collapsed;
        public Visibility VisaTid
        {
            get { return visaTid; }
            set
            {
                visaTid = value;
                OnPropertyChanged();
            }
        }

        private Visibility gömNätter = Visibility.Visible;
        public Visibility GömNätter
        {
            get { return gömNätter; }
            set
            {
                gömNätter = value;
                OnPropertyChanged();
            }
        }
        
        private bool ankomstReadOnly = false;
        public bool AnkomstReadOnly
        {
            get { return ankomstReadOnly; }
            set
            {
                ankomstReadOnly = value;
                OnPropertyChanged();
            }
        }


        private DateTime avresa;
        public DateTime Avresa
        {
            get { return avresa; }
            set {
                if (value < ankomst) avresa = Ankomst;
                else avresa = value;
                OnPropertyChanged();
            }
        }
        private bool avresaReadOnly = false;
        public bool AvresaReadOnly
        {
            get { return avresaReadOnly; }
            set
            {
                avresaReadOnly = value;
                OnPropertyChanged();
            }
        }
        private bool saveCustomerReadOnly = true;
        public bool SaveCustomerReadOnly
        {
            get { return saveCustomerReadOnly; }
            set
            {
                saveCustomerReadOnly = value;
                OnPropertyChanged();
            }
        }
        private bool stängTabortReadOnly = true;
        public bool StängTabortReadOnly
        {
            get { return stängTabortReadOnly; }
            set
            {
                stängTabortReadOnly = value;
                OnPropertyChanged();
            }
        }
        private bool checkaInReadOnly = false;
        public bool CheckaInReadOnly
        {
            get { return checkaInReadOnly; }
            set
            {
                checkaInReadOnly = value;
                OnPropertyChanged();
            }
        }
        private Visibility checkaInVisibility = Visibility.Collapsed;
        public Visibility CheckaInVisibility
        {
            get { return checkaInVisibility; }
            set
            {
                checkaInVisibility = value;
                OnPropertyChanged();
            }
        }
        private bool checkaUtReadOnly = false;
        public bool CheckaUtReadOnly
        {
            get { return checkaUtReadOnly; }
            set
            {
                checkaUtReadOnly = value;
                OnPropertyChanged();
            }
        }
        private Visibility checkaUtVisibility = Visibility.Collapsed;
        public Visibility CheckaUtVisibility
        {
            get { return checkaUtVisibility; }
            set
            {
                checkaUtVisibility = value;
                OnPropertyChanged();
            }
        }
        
        private Visibility skapabokningVisibility;
        public Visibility SkapabokningVisibility
        {
            get { return skapabokningVisibility; }
            set
            {
                skapabokningVisibility = value;
                OnPropertyChanged();
            }
        }
        
        private Visibility taBortÄndraVisability = Visibility.Collapsed;
        public Visibility TaBortÄndraVisability
        {
            get { return taBortÄndraVisability; }
            set
            {
                taBortÄndraVisability = value;
                OnPropertyChanged();
            }
        }
        private Visibility taBortVisability = Visibility.Visible;
        public Visibility TaBortVisability
        {
            get { return taBortVisability; }
            set
            {
                taBortVisability = value;
                OnPropertyChanged();
            }
        }
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// skapar bokning 
        /// </summary>
        /// <param name="ankomsttid"></param>
        /// <param name="avresetid"></param>
        /// <param name="användareID"></param>
        /// <param name="kundID"></param>
        /// <param name="facilitetID"></param>
        /// <param name="antalPersoner"></param>
        /// <returns></returns>
        public Bokning SkapaBokning(DateTime ankomsttid, DateTime avresetid, Användare användareID, Kund kundID, List<Facilitet> facilitetID, string antalPersoner)
        {
            BookingController bc = new BookingController();
            Bokning b = bc.CreateBokning(ankomsttid, avresetid, användareID, kundID, facilitetID, antalPersoner);
            return b;
        }

        /// <summary>
        /// spara bpkning till databasen, antingen sparas för första gången eller uppdateras vid ändring i bokning
        /// </summary>
        private ICommand saveCustomer = null!;

        public ICommand SaveCustomer => saveCustomer ??= saveCustomer = new RelayCommand<ICloseable>((view) =>
        {
            BookingController bc = new BookingController();
            TimeSpan bokningsLängd = Avresa - Ankomst;

            
            if (uppdateraBokning)
            {
                Bokning.Återbetalningsskydd = Avbetalningsskydd;
                Bokning.Ankomsttid = Ankomst;
                Bokning.Avresetid = Avresa;
                Bokning.AntalPersoner = AntalPersoner;
                bc.UppdateraBokning(Bokning);
                MessageBoxResult respons = MessageBox.Show($"Ändringar för bokning {Bokning.BokningsID} är nu sparad i systemet!");
                CloseCommand.Execute(view);
            }
            else
            {
                Bokning.Återbetalningsskydd = Avbetalningsskydd;
                Bokning.Bokningsstatus = Status.Kommande;
                Bokning.Totalpris = Totalpris;
                bc.SparaBokning(Bokning);
                MessageBoxResult respons = MessageBox.Show($"Bokning {Bokning.BokningsID} är nu sparad i systemet!");
                PrintController.Run(Bokning);
                //DETTA OCH konfigurera pdf i create
                CloseCommand.Execute(view);
            }
        });
        /// <summary>
        /// Tar bort bokning som skapats men inte sparats i databasen
        /// </summary>
        private ICommand stängTabort = null!;

        public ICommand StängTabort => stängTabort ??= stängTabort = new RelayCommand<ICloseable>((view) =>
        {
            Bokning = null;
            CloseCommand.Execute(view);

        });

        /// <summary>
        /// Makiulerar bokning 
        /// </summary>
        private ICommand stängTabortÄndra = null!;

        public ICommand StängTabortÄndra => stängTabortÄndra ??= stängTabortÄndra = new RelayCommand<ICloseable>((view) =>
        {
            BookingController bc = new BookingController();
            Bokning.Bokningsstatus = Status.Makulerad;
            bc.UppdateraBokning(Bokning);
            if(Bokning.Fakturaref != null)
            {
                foreach(Faktura f in Bokning.Fakturaref)
                {
                    f.Fakturastatus = Status.Makulerad;
                }
            }
            CloseCommand.Execute(view);

        });

        /// <summary>
        /// checkar in bokning
        /// </summary>
        private ICommand checkaIn = null!;
        public ICommand CheckaIn => checkaIn ??= checkaIn = new RelayCommand<ICloseable>((view) =>
        {
            //if(CheckaUtReadOnly == false)
            //{
            //    MessageBoxResult responsif = MessageBox.Show($"Du måste ha checkat in eller vara {Bokning.Avresetid} för att kunna CHECKA UT");

            //}
            BookingController bc = new BookingController();
            Bokning.Bokningsstatus = Status.Incheckad;
            bc.UppdateraBokning(Bokning);
            MessageBoxResult respons = MessageBox.Show($"Bokning {Bokning.BokningsID} är nu INCHECKAD i systemet!");
            CloseCommand.Execute(view);
        });

        /// <summary>
        /// checkar ut bokning
        /// </summary>
        private ICommand checkaUt = null!;
        public ICommand CheckaUt => checkaUt ??= checkaUt = new RelayCommand<ICloseable>((view) =>
        {
            BookingController bc = new BookingController();
            Bokning.Bokningsstatus = Status.Utcheckad;
            bc.UppdateraBokning(Bokning);
            MessageBoxResult respons = MessageBox.Show($"Bokning {Bokning.BokningsID} är nu UTCHECKAD i systemet!");
            CloseCommand.Execute(view);

        });

        /// <summary>
        /// stänger fönster
        /// </summary>
        private ICommand closeCommand = null!;

        public ICommand CloseCommand => closeCommand ??= closeCommand = new RelayCommand<ICloseable>((view) =>
        {
            view.Close();

        });


        public BookingOverviewViewModel()
        {

        }
        /// <summary>
        /// konstruktor som kör vid hantering/ändring av bokning
        /// </summary>
        /// <param name="bokning"></param>
        public BookingOverviewViewModel(Bokning bokning)
        {
            Bokningsnummer = bokning.BokningsID;
            Kund = bokning.KundID;
            Bokning = bokning;
            Ankomst = bokning.Ankomsttid;
            Avresa = bokning.Avresetid;
            AntalPersoner = Bokning.AntalPersoner;
            TimeSpan tidsspann = Avresa - Ankomst;
            AntalNätter = tidsspann.Days;
            Avbetalningsskydd = bokning.Återbetalningsskydd;
            foreach(Facilitet f in bokning.FacilitetID)
            {
                if (AntalNätter == 0 && f.FacilitetsPris.BokningTyp == "Tim")
                {
                    AntalTimmar = tidsspann.Hours;
                    Totalpris = (float)Math.Round(f.FacilitetsPris.Pris, 2) * AntalTimmar;

                }
                else if (AntalNätter == 0)
                {
                    Totalpris = (float)Math.Round(f.FacilitetsPris.Pris, 2);

                }
                else
                {
                    Totalpris = (float)Math.Round(f.FacilitetsPris.Pris, 2) * AntalNätter;
                }
            }
            VaravMoms = (float)Math.Round(Totalpris*0.2, 2);
            string Benämning = string.Empty;
            foreach (Facilitet f in  bokning.FacilitetID)
            {

                if(f.LägenhetsID != null)
                {
                    if(Benämning == string.Empty)
                    {
                        Benämning = f.LägenhetsID.LägenhetBenämning;
                    }
                    else
                    {
                        Benämning = Benämning + ", " + f.LägenhetsID.LägenhetBenämning;
                    }
                }
                if (f.CampingID != null)
                {
                    if(Benämning != string.Empty)
                    {
                        Benämning = f.CampingID.CampingBenämning;
                    }
                    else
                    {
                        Benämning = Benämning + ", " + f.CampingID.CampingBenämning;
                    }
                }
                if (f.KonferensID != null)
                {
                    if(Benämning == string.Empty) 
                    {
                        Benämning = f.KonferensID.KonferensBenämning;
                    }
                    else
                    {
                        Benämning = Benämning + ", " + f.KonferensID.KonferensBenämning;
                    }
                }
            }
            Facilitetstyp = Benämning;
            SkapabokningVisibility = Visibility.Collapsed;
            CheckaInVisibility = Visibility.Visible; 
            CheckaUtVisibility = Visibility.Visible;
            AnkomstReadOnly = true;
            AvresaReadOnly = true;
            AntalPersonerReadOnly = false;
            uppdateraBokning = true;
            taBortÄndraVisability = Visibility.Visible;
            taBortVisability = Visibility.Collapsed;
            if (Ankomst == DateTime.Today && bokning.Bokningsstatus != Status.Incheckad && bokning.Bokningsstatus != Status.Utcheckad)
            {
                CheckaInReadOnly = true;
            }
            if(Avresa.Date == DateTime.Today && bokning.Bokningsstatus == Status.Incheckad && bokning.Bokningsstatus != Status.Utcheckad) 
            {
                CheckaUtReadOnly = true;
                CheckaInReadOnly = false;

            }
        }

        /// <summary>
        /// Kundstruktor som körs när en bokning skapas, sparas först till databasen när man klickar på sparaknappen
        /// </summary>
        /// <param name="ValdKund"></param>
        /// <param name="Valdfacilitet"></param>
        /// <param name="Valdavresetid"></param>
        /// <param name="Valdankomsttid"></param>
        /// <param name="ValdaAntalPersoner"></param>
        public BookingOverviewViewModel(Kund ValdKund, Facilitet Valdfacilitet, DateTime Valdavresetid, DateTime Valdankomsttid, string ValdaAntalPersoner)
        { 
            Kund = ValdKund;
            AntalPersoner = ValdaAntalPersoner;
            Ankomst = Valdankomsttid;
            Avresa = Valdavresetid;
            TimeSpan tidsspann = Avresa - Ankomst;
            AntalNätter = tidsspann.Days;
            uppdateraBokning = false;
            if (AntalNätter == 0 && Valdfacilitet.FacilitetsPris.BokningTyp == "Tim")
            {
                AntalTimmar = tidsspann.Hours;
                Totalpris = (float)Math.Round(Valdfacilitet.FacilitetsPris.Pris, 2) * AntalTimmar;
                if(ValdKund.Rabatt != 0)
                {
                    Totalpris = totalpris * (1 - (ValdKund.Rabatt/ 100));
                }


            }
            else if(AntalNätter == 0)
            {
                Totalpris = (float)Math.Round(Valdfacilitet.FacilitetsPris.Pris, 2);
                if (ValdKund.Rabatt != 0)
                {
                    Totalpris = totalpris * (1 - (ValdKund.Rabatt / 100));
                }

            }
            else
            {
                Totalpris = (float)Math.Round(Valdfacilitet.FacilitetsPris.Pris, 2) * AntalNätter;
                if (ValdKund.Rabatt != 0)
                {
                    Totalpris = totalpris * (1 - (ValdKund.Rabatt / 100));
                }

            }
            VaravMoms = (float)(Totalpris * 0.2);
            VaravMoms = (float)Math.Round(VaravMoms, 2);
            


            if (Valdfacilitet.LägenhetsID != null)
            {
                Facilitetstyp = "Lägenhet, " + Valdfacilitet.LägenhetsID.Lägenhetstorlek;
            }
            if (Valdfacilitet.CampingID != null)
            {
                Facilitetstyp = "Campingplats, " + Valdfacilitet.CampingID.CampingBenämning;
            }
            if (Valdfacilitet.KonferensID != null && Valdfacilitet.FacilitetsPris.BokningTyp != "Tim")
            {
                Facilitetstyp = "Konferenssal, " + Valdfacilitet.KonferensID.KonferensBenämning;
            }
            if (Valdfacilitet.KonferensID != null && Valdfacilitet.FacilitetsPris.BokningTyp == "Tim")
            {
                Facilitetstyp = "Konferenssal, " + Valdfacilitet.KonferensID.KonferensBenämning;
                GömNätter = Visibility.Collapsed;
                VisaTid = Visibility.Visible;
            }
            List<Facilitet> BokadFacilitet = new List<Facilitet>
            {
                Valdfacilitet
            };
            Bokning = SkapaBokning(Ankomst, Avresa, SessionController.LoggedIn, ValdKund, BokadFacilitet, ValdaAntalPersoner);
            Bokningsnummer = Bokning.BokningsID;
            SkapabokningVisibility = Visibility.Visible;

        }
    }
}
