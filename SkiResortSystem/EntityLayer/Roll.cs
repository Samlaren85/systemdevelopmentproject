﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Roll
    {
        public string RollID { get; set; }
        public string Rolltyp { get; set; }
        public List<Behörighet> BehörighetID { get; set; }
    }
}
