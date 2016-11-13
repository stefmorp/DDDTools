using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace DDDTools
{
    class PdfGenerator
    {

        public void Create(string text,Stream filestream)
        {
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, filestream);
            document.Open();

            // adding metadata
            document.AddAuthor("Fabrizio Spoleti");
            document.AddCreator("DDDTools");
            document.AddTitle("Reciept");

            // write to document
            document.Open();

            document.Add(new Paragraph(text));

            document.Close();

            writer.Close();

            filestream.Close();

        }

        public void FillTemplate(string path, string number, string year, string name, string surname, string address, string cap, string city, string province, string fiscalcode, string IVA, string amount, string date)
        {
            string oldFile = @"..\..\data\template.pdf";
            string newFile = path;

            // open the reader
            PdfReader reader = new PdfReader(oldFile);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);

            // open the writer
            FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            // the pdf content
            PdfContentByte canvas = writer.DirectContent;

            // select the font properties
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            canvas.SetColorFill(BaseColor.DARK_GRAY);
            canvas.SetFontAndSize(bf, 11);

            // write the text in the pdf content
            canvas.BeginText();

            decimal decimal_amount = Decimal.Parse(amount);
            

            int val = Convert.ToInt32(Math.Floor(decimal_amount));

            int decimal_part = (int)((decimal_amount - (int)decimal.Truncate(decimal_amount)) * 100);


            NumberToWords words = new NumberToWords(val);
            string amountinwords = words.GetString(decimal_part.ToString());
            Console.WriteLine(amountinwords);

            canvas.ShowTextAligned(Element.ALIGN_LEFT, surname + " " + name, 100, 630, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, address, 100, 607, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, city, 100, 582, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, fiscalcode, 100, 559, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, IVA, 100, 533, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, cap, 420, 607, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, province, 435, 581, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, decimal_amount.ToString("F"), 115, 405, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, amountinwords, 250, 405, 0);
            double d = double.Parse(date);
            DateTime conv = DateTime.FromOADate(d);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, "Ricevuta n'" + number + " del " + conv.ToShortDateString(), 34, 349, 0);



            canvas.EndText();

            // create the new page and add it to the pdf
            PdfImportedPage page = writer.GetImportedPage(reader, 1);
            canvas.AddTemplate(page, 0, 0);

            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();
        }

    }
}
