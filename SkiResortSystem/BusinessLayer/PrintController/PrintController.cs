using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.PrintController
{
    public class Util
    {
        internal static string GetPath(string filePath)
        {
            var exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return System.IO.Path.Combine(appRoot, filePath);
        }

    }
    public class PrintController
    {
        public PrintController()
        {

        }
        public static void Run(Bokning bokning)
        {
            Document document = new Document();
            Page page = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
            document.Pages.Add(page);
            string x = "Nej";
            if (bokning.Återbetalningsskydd == true)
            {
                x = "Ja";
            }
            string labelText = $"Bokningsbekräftelse\n\nBokningsnummer: {bokning.BokningsID}\nKund: {bokning.KundID.Namn}\nAnkomsttid: {bokning.Ankomsttid}\nAvresetid: {bokning.Avresetid}\nÅterbetalningsskydd: {x}";
            Label label = new Label(labelText, 0, 0, 504, 700, Font.Helvetica, 18, TextAlign.Center);
            page.Elements.Add(label);
            string uniqueFileName = $"Bokningsbekräftelse_{bokning.BokningsID}.pdf";
            document.Draw(Util.GetPath($"PrintController/Bokningsbekräftelser/{uniqueFileName}"));
        }
        public static void Run(Faktura faktura)
        {
            Document document = new Document();
            Page page = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
            document.Pages.Add(page);
            string labelText = $"Fakturadatum: {faktura.Fakturadatum}\t\t\t\t\t\t\t\t\tFakturanummer: {faktura.FakturaID}\n\nFörfallodatum: {faktura.Förfallodatum}\n\n\n" +
                $"Totalpris:{faktura.Totalpris}\n\nStatus: {faktura.Fakturastatus}\n\nKund: {faktura.Bokningsref.KundID}\n\nMoms: {faktura.Moms}";
            Label label = new Label(labelText, 0, 0, 704, 800, Font.Helvetica, 12, TextAlign.Left);
            page.Elements.Add(label);
            string uniqueFileName = $"Faktura_{faktura.FakturaID}.pdf";
            document.Draw(Util.GetPath($"PrintController/Fakturor/{uniqueFileName}"));
        }
    }
}
