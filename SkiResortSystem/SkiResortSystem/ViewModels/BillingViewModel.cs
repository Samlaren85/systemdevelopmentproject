using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResortSystem.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private Bokning selectFaktura;
        public Bokning SelectFaktura
        {
            get { return selectFaktura; }
            set
            {
                selectFaktura = value;
            }
        }

        private Faktura selectedFaktura;
        public Faktura SelectedFaktura
        {
            get { return selectedFaktura; }
            set
            {
                if (value == selectedFaktura) return;
                if (value != null)
                {
                    selectedFaktura = value;
                    SearchBills();
                }
                OnPropertyChanged();
            }
        }

        private string searchFaktura;
        public string SearchFaktura
        {
            get { return searchFaktura; }
            set
            {
                if (value != null)
                {
                    searchFaktura = value;
                    SearchBills(); if (searchFaktura == string.Empty) { FakturaResults = new List<Faktura>(); }
                    OnPropertyChanged(SearchFaktura);
                }
            }
        }

        private IList<Faktura> fakturaResults;
        public IList<Faktura> FakturaResults
        {
            get { return fakturaResults; }
            set
            {
                fakturaResults = value;
                OnPropertyChanged();
            }
        }
        public void SearchBills()
        {
            EconomyController ec = new EconomyController();
            FakturaSökning = ec.FindFaktura(SelectFaktura);
        }

        private Faktura fakturaSökning;
        public Faktura FakturaSökning
        {
            get { return fakturaSökning; }
            set
            {
                fakturaSökning = value;
                OnPropertyChanged();
            }
        }
    }
}
