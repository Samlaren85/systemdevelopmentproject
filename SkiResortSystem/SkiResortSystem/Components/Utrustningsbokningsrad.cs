using SkiResortSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DateTime FrånDatum { get; set; }
        public DateTime TillDatum { get; set; }
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
