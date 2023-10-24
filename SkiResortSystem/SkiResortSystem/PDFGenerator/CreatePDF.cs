using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using EntityLayer;
using System.Collections.Generic;
using System;

namespace DynamicPDFCoreSuite.Examples
{
    class CreatePDF
    {
        public static void Run(Bokning bokning)
        {
            Document document = new Document();
            Page page = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
            document.Pages.Add(page);
            string x = "Nej";
            if(bokning.Återbetalningsskydd == true)
            {
                x = "Ja";
            }
            string labelText = $"Bokningsbekräftelse\n\nBokningsnummer: {bokning.BokningsID}\nKund: {bokning.KundID.Namn}\nAnkomsttid: {bokning.Ankomsttid}\nAvresetid: {bokning.Avresetid}\nÅterbetalningsskydd: {x}";
            Label label = new Label(labelText, 0, 0, 504, 700, Font.Helvetica, 18, TextAlign.Center);
            page.Elements.Add(label);
            string uniqueFileName = $"Bokningsbekräftelse_{bokning.BokningsID}.pdf";
            document.Draw(Util.GetPath($"PDFGenerator/Bokningsbekräftelser/{uniqueFileName}"));
        }
        public static void Run(Faktura faktura, Faktura faktura2)
        {   List<Faktura> fakturorförutskrift = new List<Faktura>();
            fakturorförutskrift.Add(faktura);
            fakturorförutskrift.Add(faktura2);
            foreach(Faktura f in fakturorförutskrift)
            {
                Document document = new Document();
                Page page = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
                document.Pages.Add(page);
                string labelText = $"Fakturadatum: {f.Fakturadatum}\t\t\t\t\t\t\t\t\tFakturanummer: {f.FakturaID}\n\nFörfallodatum: {f.Förfallodatum}\n\n\n" +
                    $"Totalpris:{f.Totalpris}\n\nStatus: {f.Fakturastatus}\n\nKund: {f.Bokningsref.KundID}\n\nMoms: {f.Moms}";
                Label label = new Label(labelText, 0, 0, 704, 800, Font.Helvetica, 12, TextAlign.Left);
                page.Elements.Add(label);
                string uniqueFileName = $"Faktura_{f.FakturaID}.pdf";
                document.Draw(Util.GetPath($"Fakturor/{uniqueFileName}"));
            }
           
        }
    }
}
