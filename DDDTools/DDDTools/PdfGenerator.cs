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

    }
}
