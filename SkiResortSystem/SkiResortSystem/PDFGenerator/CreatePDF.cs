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
            if(bokning.Återbetalningsskydd = true)
            {
                x = "Ja";
            }
            string labelText = $"Bokningsbekräftelse\n\nBokningsnummer: {bokning.BokningsID}\nKund: {bokning.KundID.Namn}\nAnkomsttid: {bokning.Ankomsttid}\nAvresetid: {bokning.Avresetid}\nÅterbetalningsskydd: {x}";
            Label label = new Label(labelText, 0, 0, 504, 500, Font.Helvetica, 18, TextAlign.Center);
            page.Elements.Add(label);
            string uniqueFileName = $"Bokningsbekräftelse_{bokning.BokningsID}.pdf";
            document.Draw(Util.GetPath($"Bokningsbekräftelser/{uniqueFileName}"));
        }
    }
}
//public float UtnyttjadKredit { get; set; }
//public bool Återbetalningsskydd { get; set; }
//public DateTime Ankomsttid { get; set; }
//public DateTime Avresetid { get; set; }
//public string AntalPersoner { get; set; }
//public Användare AnvändareID { get; set; }
//public Kund KundID { get; set; }

//public ICollection<Facilitet> FacilitetID { get; set; }

//public ICollection<Utrustningsbokning>? UtrustningRef { get; set; }

//public ICollection<Aktivitetsbokning>? AktivitetRef { get; set; }