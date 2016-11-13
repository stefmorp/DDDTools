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
            string oldFile = @"C:\Users\lorenzo\Documents\DDDTools\DDDTools\DDDTools\data\template.pdf";
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
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            canvas.SetColorFill(BaseColor.DARK_GRAY);
            canvas.SetFontAndSize(bf, 10);

            // write the text in the pdf content
            canvas.BeginText();

            // testing cooridinates
            //string text = "Some random blablablabla...";
            // put the alignment and coordinates here
            //for(int i = 0; i < 1000; i = i + 50)
            //{
            //    for(int j = 0; j < 1000; j = j + 50)
            //    {
            //        canvas.ShowTextAligned(1, i.ToString() + "/" + j.ToString(), i, j, 0);
            //    }
            //}
            NumberToWords words = new NumberToWords(Int32.Parse(amount));
            string amountinwords = words.GetString();
            Console.WriteLine(amountinwords);

            canvas.ShowTextAligned(Element.ALIGN_LEFT, surname + " " + name, 105, 630, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, address, 105, 607, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, city, 70, 582, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, fiscalcode, 70, 559, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, IVA, 70, 535, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, cap, 420, 607, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, province, 448, 581, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, amount, 115, 405, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, amountinwords, 250, 405, 0);
            canvas.ShowTextAligned(Element.ALIGN_LEFT, "Ricevuta n'" + number + " del " + date , 150, 349, 0);



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
