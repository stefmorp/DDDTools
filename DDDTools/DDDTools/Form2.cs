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


                var record = Dumbo.GetById(this.id.Text);
                if (record != null && record.Count >= 12)
                {
                    pdf.FillTemplate(pdfPath, record[0], record[1], record[2], record[3], record[4], record[5],
                       record[6], record[7], record[8], record[9], record[10], record[11]);
                }
                else
                {
                    MessageBox.Show("Record non trovato!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
            try
            {
                Update1(40);
                Dumbo.Update(path);
                Update1(100);
            }
            catch (Exception ex)
            {
                // Handle exceptions on background thread - show error on UI thread
                if (InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show(
                            $"Error loading Excel file:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                            "Excel Load Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show(
                        $"Error loading Excel file:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                        "Excel Load Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
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

        private void receiptbutton2_Click(object sender, EventArgs e)
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


                var record = Dumbo.GetById(this.id.Text);
                if (record != null && record.Count >= 12)
                {
                    pdf.FillTemplate2(pdfPath, record[0], record[1], record[2], record[3], record[4], record[5],
                       record[6], record[7], record[8], record[9], record[10], record[11]);
                }
                else
                {
                    MessageBox.Show("Record non trovato!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Console.WriteLine("Created Pdf");

            }

        }
    }
}
