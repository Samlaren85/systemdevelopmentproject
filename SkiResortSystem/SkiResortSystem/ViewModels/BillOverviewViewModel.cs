using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    /// <summary>
    /// Vymodell för fakturaöversikten.
    /// </summary>
    class BillOverviewViewModel : ObservableObject
    {
        private Faktura faktura;
        public Faktura Faktura
        {
            get { return faktura; }
            set
            {
                faktura = value;
                OnPropertyChanged();
            }
        }
        private string privFöre;

        public string PrivFöre
        {
            get { return privFöre; }
            set { privFöre = value; }
        }

        private ObservableCollection<string> artikel;
        public ObservableCollection<string> Artikel
        {
            get { return artikel; }
            set
            {
                artikel = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> pris;
        public ObservableCollection<string> Pris
        {
            get { return pris; }
            set
            {
                pris = value;
                OnPropertyChanged();
            }
        }
        private Bokning bokning;
        public Bokning Bokning
        {
            get { return bokning; }
            set
            {
                bokning = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Knapp som arkiverar faktura, sätter fakturastatus till makulerad
        /// </summary>
        private ICommand arkiveraFaktura = null!;
        public ICommand ArkiveraFaktura =>
            arkiveraFaktura ??= arkiveraFaktura = new RelayCommand<ICloseable>((view) =>
            {
                Faktura.Fakturastatus = Status.Makulerad;
                EconomyController ec = new EconomyController();
                ec.UpdateFaktura(Faktura);
                MessageBoxResult respons = MessageBox.Show($"Faktura {Faktura.FakturaId} är nu arkiverad i systemet!");
                CloseCommand.Execute(view);
            });

        /// <summary>
        /// Command som stänger fönstret
        /// </summary>
        private ICommand closeCommand = null!;
        public ICommand CloseCommand =>
            closeCommand ??= closeCommand = new RelayCommand<ICloseable>((view) =>
            {
                view.Close();
            });

        public BillOverviewViewModel()
        {
        }
        /// <summary>
        /// Teldelar värdena från fakturan till listorna som presenteras och ger properties sina värden.(Summa summarum: Allt som ska presenteras på översikten)
        /// </summary>
        /// <param name="faktura"></param>
        public BillOverviewViewModel(Faktura faktura)
        {
            Artikel = new ObservableCollection<string>();
            Pris = new ObservableCollection<string>();

            Faktura = faktura;
            Bokning = faktura.Bokningsref;
            if(Bokning.KundID.Företagskund != null)
            {
                PrivFöre = "Organisationsnummer";
            }
            else
            {
                PrivFöre = "Personnummer";
            }
            if(Bokning.FacilitetID != null)
            {
                foreach (Facilitet f in Bokning.FacilitetID)
                {
                    if (f.Typ != null)
                    {
                        Artikel.Add(f.Typ);

                    }
                    if (f.FacilitetsPris != null)
                    {
                        Pris.Add(f.TotalprisFörPresentationIBoendeModul.ToString());

                    }
                }
            }
            if (Bokning.Fakturaref != null && Bokning.Återbetalningsskydd.Equals(true))
            {
                Faktura fakturaFörKontroll = Bokning.Fakturaref.Min();
                int result = faktura.CompareTo(fakturaFörKontroll);
                if (result.Equals(1))
                {
                    string avbeställningsskydd = "Avbokningsskydd";
                    string prisÅb = "300";
                    Artikel.Add(avbeställningsskydd);
                    Pris.Add(prisÅb);
                }
            }
           
            if(Bokning.UtrustningRef != null)
            {
                foreach (Utrustningsbokning u in Bokning.UtrustningRef)
                {
                    if (u.Utrustning.UtrustningsBenämning != null)
                    {
                        Artikel.Add(u.Utrustning.UtrustningsBenämning);

                    }
                    if (u.Utrustning.Pris != null)
                    {
                        Pris.Add(u.Utrustning.Pris.ToString());

                    }
                }
            }
            if(Bokning.AktivitetRef != null)
            {
                foreach (Aktivitetsbokning a in Bokning.AktivitetRef)
                {
                    if (a.Aktivitetsref.Typ != null)
                    {
                        Artikel.Add(a.Aktivitetsref.Typ);

                    }
                    if (a.TotalPris != null)
                    {
                        Pris.Add(a.TotalPris.ToString());

                    }
                }
            }
            OnPropertyChanged(nameof(Artikel));
            OnPropertyChanged(nameof(Pris));


        }
    }
}
