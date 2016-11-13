using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace DDDTools
{
    public partial class Form2 : Form
    {



        DataProcesser Dumbo = new DataProcesser();
        private String datatablePath = "" ;
        private String pdfPath = "";

        public Form2()
        {
            InitializeComponent();
        }

        private void receiptbutton_Click(object sender, EventArgs e)
        {
            PdfGenerator pdf = new PdfGenerator();

            //var FD = new System.Windows.Forms.OpenFileDialog();
            //if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    string fileToOpen = FD.FileName;
            //    Console.WriteLine(fileToOpen);
            //}

            var SD = new System.Windows.Forms.SaveFileDialog();
            SD.DefaultExt = "pdf";
            SD.AddExtension = true;
            //openFileDialog1.InitialDirectory = "c:\\";
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //openFileDialog1.FilterIndex = 2;
            SD.RestoreDirectory = true;

            if (SD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {


                pdfPath = SD.FileName;

                Console.WriteLine(pdfPath);

                //String path = @"C:\Users\lorenzo\Documents\DDDTools\DDDTools\DDDTools\data\receipt.pdf";


                pdf.FillTemplate(pdfPath, Dumbo.Get()[this.id.Text][0], Dumbo.Get()[this.id.Text][1], Dumbo.Get()[this.id.Text][2], Dumbo.Get()[this.id.Text][3], Dumbo.Get()[this.id.Text][4], Dumbo.Get()[this.id.Text][5],
                   Dumbo.Get()[this.id.Text][6], Dumbo.Get()[this.id.Text][7], Dumbo.Get()[this.id.Text][8], Dumbo.Get()[this.id.Text][9], Dumbo.Get()[this.id.Text][10], Dumbo.Get()[this.id.Text][11]);

                Console.WriteLine("Created Pdf");

            }
        }

        private void OpenPdf_Click(object sender, EventArgs e)
        {
            if (datatablePath != "")
            {
                System.Diagnostics.Process.Start(pdfPath);
            }

            
        }

        private void OpenXlsx_Click(object sender, EventArgs e)
        {
            if(datatablePath != "")
            {
                System.Diagnostics.Process.Start(datatablePath);
            }
            
        }

        private void UpdateData_Click(object sender, EventArgs e)
        {
            var OD = new System.Windows.Forms.OpenFileDialog();

            if (OD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                datatablePath = OD.FileName;


                //Thread thread = new Thread(DumboUpdate(path));
                Thread thread = new Thread(() => DumboUpdate(datatablePath));
                thread.IsBackground = true;
                thread.Start();
                // Dumbo.Update();

            }



        }

        private void DumboUpdate(String path)
        {
            Update1(40);
            Dumbo.Update(path);
            Update1(100);

        }

        public void Update1(int i)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<int>(Update1), new object[] { i });
                return;
            }
            //double progress = (double)i;
            //progress = (progress / 584) * 100;
            //i = (int)progress;
            progressBar1.Value = i;
        }
    }
}
