using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public enum Status
    {
        //Generell första status 
        Kommande = 0,

        //Statusar för boende
        Incheckad = 1, 
        Utcheckad = 2, 

        //Statusar för utrustning
        Utlämnad = 10,
        Inrapporterad = 11,

        //Statusar för aktiviteter
        Genomförd = 20,

        //Generell makulerad status 
        Makulerad = 99

    }
}
