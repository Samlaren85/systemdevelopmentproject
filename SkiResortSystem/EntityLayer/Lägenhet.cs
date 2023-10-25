﻿using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Lägenhet
    {
        private static int _antalLägenheter = 0;
        [Key]
        public string LägenhetID { get; set; }
        public string LägenhetBenämning { get; set; }
        public int Bäddar { get; set; }
        public string Lägenhetstorlek { get; set; }
        public Lägenhet()
        {
            
        }
        public Lägenhet(string lägenhetbenämning, int bäddar, string lägenhetstorlek)
        {
            _antalLägenheter++;
            LägenhetID = "LGH" + _antalLägenheter.ToString("0000");
            LägenhetBenämning = lägenhetbenämning;
            Bäddar = bäddar;
            Lägenhetstorlek = lägenhetstorlek;
        }
    }
}
