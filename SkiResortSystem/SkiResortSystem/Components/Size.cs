﻿using SkiResortSystem.Models;

namespace SkiResortSystem.Components
{
    public class Size : ObservableObject
    {
        private string storlek;
        public string Storlek 
        { 
            get { return storlek; }
            set
            {
                storlek = value;
                OnPropertyChanged();
            }
        }
        private int antal;
        public int Antal 
        {
            get { return antal; }
            set
            {
                antal = value;
                OnPropertyChanged();
            } 
        }
        public Size(string stl)
        {
            Storlek = stl;
            Antal = 0;
        }
    }
}
