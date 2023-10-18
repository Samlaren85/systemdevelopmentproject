﻿using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private IList<Bokning> hämtadeBokningarAttFakturera { get; set; }
        public IList<Bokning> HämtadeBokningarAttFakturera 
        {
            get { return hämtadeBokningarAttFakturera; }
            set
            {
                hämtadeBokningarAttFakturera = value;
                OnPropertyChanged();
            }
        }

        private Bokning skapandeAvFakturor { get; set; }
        public Bokning SkapandeAvFakturor 
        {
            get { return skapandeAvFakturor; }
            set
            {
                skapandeAvFakturor = value;
                OnPropertyChanged();
            }
        }
        

        private Faktura selectFaktura;
        public Faktura SelectFaktura
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
            //FakturaSökning = ec.FindFaktura(SelectFaktura);
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

        private List<Faktura> faktureradeFakturor {  get; set; }
        public List<Faktura> FaktureradeFakturor 
        {
            get { return faktureradeFakturor; }
            set
            {
                faktureradeFakturor = value;
                OnPropertyChanged();
            }
        }

        private string sökFaktureradeFakturor { get; set; }
        public string SökFaktureradeFakturor
        {
            get { return sökFaktureradeFakturor; }
            set
            {
                sökFaktureradeFakturor= value;
                HämtaFaktureradeFakturor.Execute(true);
                OnPropertyChanged();
            }
        }
        private ICommand createFaktura = null!;
        public ICommand CreateFaktura =>
            createFaktura ??= createFaktura = new RelayCommand(() =>
            {
                EconomyController ec = new EconomyController();
                ec.CreateFaktura(SkapandeAvFakturor);
            }
            );
        private ICommand fetchFaktura = null!;
        public ICommand FetchFaktura => fetchFaktura ??= fetchFaktura = new RelayCommand(() =>
        {
            BookingController bc = new BookingController();
            HämtadeBokningarAttFakturera = bc.FindMasterBooking();
        }
        );

        private ICommand hämtaFaktureradeFakturor = null!;
        public ICommand HämtaFaktureradeFakturor => hämtaFaktureradeFakturor ??= hämtaFaktureradeFakturor = new RelayCommand(() =>
        {
            EconomyController ec = new EconomyController();
            BookingController bc = new BookingController();
            List<Bokning> Lista = bc.FindMasterBooking(SökFaktureradeFakturor).ToList();
            FaktureradeFakturor = ec.HämtaFaktureradeFakturor(Lista);
        }
        );
       /* private ICommand doubleClickBillingCommand = null!;
        public ICommand DoubleClickBillingCommand =>
            doubleClickBillingCommand ??= doubleClickBillingCommand = new RelayCommand(() =>
            {
                if (SelectedFaktura != null)
                {
                    BookingOverviewViewModel bokningsöversikt = new BookingOverviewViewModel(SelectedCustomer, SelectedFacility, Avresetid, Ankomsttid, antalpersoner, Facilitetspriset);
                    windowService.ShowDialog(bokningsöversikt);
                }
            });*/
    }
}
