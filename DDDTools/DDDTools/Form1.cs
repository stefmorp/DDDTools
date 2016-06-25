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
    public partial class MainWindow : Form
    {

        DataProcesser Dumbo = new DataProcesser();

        public MainWindow()
        {
            InitializeComponent();

            

            Dumbo.Update();

            // Dumbo.Store("Lorenzo","Spoleti","0001","2011","Via culo sporco 11","2020","Aruschio","CU","GG629FGE9GH9","","500","16/4/478");
            // Dumbo.Print();

        }

        private void storebutton_Click(object sender, EventArgs e)
        {
            int key = Dumbo.getLastId() + 1;
            Dumbo.Store((key).ToString(),this.number.Text,this.year.Text,this.name.Text,this.surname.Text,this.address.Text,
                this.cap.Text,this.city.Text,this.province.Text,this.fiscalcode.Text,this.iva.Text,this.amount.Text,this.date.Text);

            Console.Write("Added: ");
            Console.Write(key);
            foreach (string s in Dumbo.Get()[key.ToString()])
            {
                Console.Write(" " + s);
            }
            Console.Write("\n");

            Dumbo.Write((key).ToString(), this.number.Text, this.year.Text, this.name.Text, this.surname.Text, this.address.Text,
                this.cap.Text, this.city.Text, this.province.Text, this.fiscalcode.Text, this.iva.Text, this.amount.Text, this.date.Text);

            Console.WriteLine("Wrote to file");
        }


    }
}
