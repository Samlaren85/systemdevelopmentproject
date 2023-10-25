using SkiResortSystem.Models;
using System;
using System.Collections.Generic;

namespace SkiResortSystem.Components
{
    /// <summary>
    /// Används för att bygga upp de attribut som behövs för att populera Utrustningsbokning när man skall boka utrustning
    /// </summary>
    public class Utrustningsbokningsrad : ObservableObject
    {
        private string typAvUtrustning;
        public string TypAvUtrustning 
        { 
            get { return typAvUtrustning; }
            set
            {
                typAvUtrustning = value;
                OnPropertyChanged();
            }
        }
        public List<string> TypAvUtrustningar { get; set; }
        
        private DateTime frånDatum;
        public DateTime FrånDatum
        {
            get { return frånDatum; }
            set
            {
                frånDatum = value;
                OnPropertyChanged();
            }
        }
        private DateTime tillDatum;
        public DateTime TillDatum
        {
            get { return tillDatum; }
            set
            {
                tillDatum = value;
                OnPropertyChanged();
            }
        }
        private int antal;
        public int Antal 
        { 
            get {  return antal; }
            set
            {
                antal = value; 
                OnPropertyChanged();
            } 
        }
        private List<string> storlekar;
        public List<string> Storlekar 
        {
            get { return storlekar; }
            set 
            {
                storlekar = value;
                Antal = Storlekar.Count;
                OnPropertyChanged();
            }
        }
        public float Pris { get; set; }
    }
}
