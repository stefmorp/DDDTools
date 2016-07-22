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

        public void FillTemplate(string number, string year, string name, string surname, string address, string cap, string city, string province, string fiscalcode, string IVA, string amount, string date)
        {
            string oldFile = @"C:\Users\Loren\Source\Repos\DDDTools\DDDTools\DDDTools\data\template.pdf";
            string newFile = @"C:\Users\Loren\Source\Repos\DDDTools\DDDTools\DDDTools\data\receipt.pdf";

            // open the reader
            PdfReader reader = new PdfReader(oldFile);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);

            // open the writer
            FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            // the pdf content
            PdfContentByte cb = writer.DirectContent;

            // select the font properties
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 10);

            // write the text in the pdf content
            cb.BeginText();

            // testing cooridinates
            //string text = "Some random blablablabla...";
            // put the alignment and coordinates here
            //for(int i = 0; i < 1000; i = i + 50)
            //{
            //    for(int j = 0; j < 1000; j = j + 50)
            //    {
            //        cb.ShowTextAligned(1, i.ToString() + "/" + j.ToString(), i, j, 0);
            //    }
            //}
            NumberToWords words = new NumberToWords(Int32.Parse(amount));
            string amountinwords = words.GetString();
            Console.WriteLine(amountinwords);

            cb.ShowTextAligned(1, surname + " " + name, 140, 630, 0);
            cb.ShowTextAligned(1, address, 130, 607, 0);
            cb.ShowTextAligned(1, city, 100, 582, 0);
            cb.ShowTextAligned(1, fiscalcode, 110, 558, 0);
            cb.ShowTextAligned(1, IVA, 100, 540, 0);
            cb.ShowTextAligned(1, cap, 430, 608, 0);
            cb.ShowTextAligned(1, province, 450, 582, 0);
            cb.ShowTextAligned(1, amount, 120, 405, 0);
            cb.ShowTextAligned(1, amountinwords, 300, 405, 0);
            cb.ShowTextAligned(1, "Ricevuta n'" + number + " del " + date , 150, 350, 0);



            cb.EndText();

            // create the new page and add it to the pdf
            PdfImportedPage page = writer.GetImportedPage(reader, 1);
            cb.AddTemplate(page, 0, 0);

            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();
        }

    }
}
