using BusinessLayer;
using ceTe.DynamicPDF.PageElements.BarCoding;
using DynamicPDFCoreSuite.Examples;
using EntityLayer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
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
        private Faktura personnummer;
        public Faktura Personnummer
        {
            get { return personnummer; }
            set
            {
                personnummer = value;
                OnPropertyChanged();
            }
        }
        private Faktura förnamn;
        public Faktura Förnamn
        {
            get { return förnamn; }
            set
            {
                förnamn = value;
                OnPropertyChanged();
            }
        }
        private Faktura efternamn;
        public Faktura Efternamn
        {
            get { return efternamn; }
            set
            {
                efternamn = value;
                OnPropertyChanged();
            }
        }
        private Faktura gatuadress;
        public Faktura Gatuadress
        {
            get { return gatuadress; }
            set
            {
                gatuadress = value;
                OnPropertyChanged();
            }
        }
        private Faktura postnummer;
        public Faktura Postnummer
        {
            get { return postnummer; }
            set
            {
                postnummer = value;
                OnPropertyChanged();
            }
        }
        private Faktura postort;
        public Faktura Postort
        { 
            get { return postort; }
            set
            {
                postort = value;
                OnPropertyChanged();
            }
        }


        private ICommand arkiveraFaktura = null!;
        public ICommand ArkiveraFaktura =>
            arkiveraFaktura ??= arkiveraFaktura = new RelayCommand<ICloseable>((view) =>
            {
                Faktura.Fakturastatus = Status.Makulerad;
                EconomyController ec = new EconomyController();
                ec.UpdateFaktura(Faktura);
                MessageBoxResult respons = MessageBox.Show($"Faktura {Faktura.FakturaID} är nu arkiverad i systemet!");
                CloseCommand.Execute(view);
            });


        private ICommand skrivUtFaktura = null!;
        public ICommand SkrivUtFaktura =>
            skrivUtFaktura ??= skrivUtFaktura = new RelayCommand(() =>
            {
                CreatePDF.Run(Faktura);
            });

        private ICommand closeCommand = null!;
        public ICommand CloseCommand =>
            closeCommand ??= closeCommand = new RelayCommand<ICloseable>((view) =>
            {
                view.Close();
            });

        public BillOverviewViewModel()
        {
        }
        public BillOverviewViewModel(Faktura faktura)
        {
            Faktura = faktura;
            
        }
    }
}
