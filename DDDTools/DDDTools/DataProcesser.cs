using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Isam.Esent.Collections.Generic;


namespace DDDTools
{
    class DataProcesser { 

        private Dictionary<Tuple<string, string>, List<string>> database = new Dictionary<Tuple<string, string>, List<string>>();
        private string lastmodified = "22/06/2016"; 

        // the fullname (a tuple containing name and surname), the number of the transaction,... (explaines itself) ... IVA is 'partita iva', amount is in Euro and the date of the transaction
        public void Store(string name, string surname, string number,string year,string address,string cap, string city, string province, string fiscalcode, string IVA, string amount, string date)
        {
            database.Add(new Tuple<string, string>(name, surname), new List<string> { number, year, address, cap, city, province, fiscalcode, IVA, amount, date});
            lastmodified = DateTime.Now.ToString("dd/MM/yyyy");
        }
       
        public void Print()
        {
            foreach(Tuple<string, string> key in database.Keys)
            {
                Console.Write(key.Item1 + " " + key.Item2);

                foreach (string s in database[key])
                {
                    Console.Write(" " + s);
                }
                Console.Write("\n");

            }
            Console.WriteLine("Lastmodified: " + lastmodified);
        }

        
    }
}
