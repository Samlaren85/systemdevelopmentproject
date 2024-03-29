﻿namespace EntityLayer
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
        Makulerad = 99,

        //Statusar för fakturor
        Obetald = 30,
        Betald = 31,
        Ofakturerad = 32,
    }
}
