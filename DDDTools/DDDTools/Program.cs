using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDDTools
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 


        [STAThread]
        static void Main()
        {
           
            DataProcesser Dumbo = new DataProcesser();

            Dumbo.Update();

            // Dumbo.Store("Lorenzo","Spoleti","0001","2011","Via culo sporco 11","2020","Aruschio","CU","GG629FGE9GH9","","500","16/4/478");
            Dumbo.Print();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());

        }
    }
}
