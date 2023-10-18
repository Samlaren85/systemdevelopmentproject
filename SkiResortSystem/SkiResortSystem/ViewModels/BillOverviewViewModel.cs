﻿using BusinessLayer;
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
        private List<string> artikel;
        public List<string> Artikel
        {
            get { return artikel; }
            set
            {
                artikel = value;
                OnPropertyChanged();
            }
        }
        private List<string> pris;
        public List<string> Pris
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
            Bokning = faktura.Bokningsref;
            foreach(Facilitet f in Bokning.FacilitetID)
            {
                Artikel.Add(f.Typ);
                Pris.Add(f.Facilitetspris.ToString());
            }
            foreach(Utrustningsbokning u in Bokning.UtrustningRef)
            {
                Artikel.Add(u.Utrustning.UtrustningsBenämning);
                Pris.Add(u.Utrustning.Pris.ToString());
            }
            foreach(Aktivitetsbokning a in Bokning.AktivitetRef)
            {
                Artikel.Add(a.Aktivitetsref.Typ);
                Pris.Add(a.TotalPris.ToString());
            }
            
        }
    }
}
