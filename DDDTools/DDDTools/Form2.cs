using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DDDTools
{
    public partial class Form2 : Form
    {

        DataProcesser Dumbo = new DataProcesser();

        public Form2()
        {
            InitializeComponent();
        }

        private void receiptbutton_Click(object sender, EventArgs e)
        {
            PdfGenerator pdf = new PdfGenerator();
            // FileStream fs = new FileStream(@"C:\Users\Loren\Source\Repos\DDDTools\DDDTools\DDDTools\data\receipt.pdf", FileMode.Create);

            pdf.FillTemplate(Dumbo.Get()[this.id.Text][0], Dumbo.Get()[this.id.Text][1], Dumbo.Get()[this.id.Text][2], Dumbo.Get()[this.id.Text][3], Dumbo.Get()[this.id.Text][4], Dumbo.Get()[this.id.Text][5],
               Dumbo.Get()[this.id.Text][6], Dumbo.Get()[this.id.Text][7], Dumbo.Get()[this.id.Text][8], Dumbo.Get()[this.id.Text][9], Dumbo.Get()[this.id.Text][10], Dumbo.Get()[this.id.Text][11]);

            Console.WriteLine("Created Pdf");
        }

        private void OpenPdf_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Users\Loren\Source\Repos\DDDTools\DDDTools\DDDTools\data\receipt.pdf");
        }

        private void OpenXlsx_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Users\Loren\Source\Repos\DDDTools\DDDTools\DDDTools\data\datatable.xlsx");
        }

        private void UpdateData_Click(object sender, EventArgs e)
        {
            Dumbo.Update();
        }
    }
}
