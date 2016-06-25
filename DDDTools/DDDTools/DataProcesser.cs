using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;

namespace DDDTools
{
    class DataProcesser
    {

        private Dictionary<Tuple<string, string>, List<string>> database = new Dictionary<Tuple<string, string>, List<string>>();
        private string lastmodified = "22/06/2016";
        FileInfo datatable = new FileInfo(@"C:\Users\Loren\Source\Repos\DDDTools\DDDTools\DDDTools\data\datatable.xlsx");



        public void Update()
        {
            using (ExcelPackage xlPackage = new ExcelPackage(datatable))
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];

                // output the data in column 1
                for (int row = 1; row < 5; row++)
                {

                    Store(worksheet.Cell(row, 3).Value, worksheet.Cell(row, 4).Value, worksheet.Cell(row, 1).Value, worksheet.Cell(row, 2).Value,
                        worksheet.Cell(row, 5).Value, worksheet.Cell(row, 6).Value, worksheet.Cell(row, 7).Value, worksheet.Cell(row, 8).Value,
                        worksheet.Cell(row, 9).Value, worksheet.Cell(row, 10).Value, worksheet.Cell(row, 11).Value, worksheet.Cell(row, 12).Value);
                    //for (int col = 1; col < 13; col++)
                    //{
                    // Console.WriteLine("Cell({0},{1}).Value={2}", row, col, worksheet.Cell(row, col).Value);
                    //}
                }


            } // the using statement calls Dispose() which closes the package.
        }

        // the fullname (a tuple containing name and surname), the number of the transaction,... (explaines itself) ... IVA is 'partita iva', amount is in Euro and the date of the transaction
        public void Store(string name, string surname, string number, string year, string address, string cap, string city, string province, string fiscalcode, string IVA, string amount, string date)
        {
            database.Add(new Tuple<string, string>(name, surname), new List<string> { number, year, address, cap, city, province, fiscalcode, IVA, amount, date });
            lastmodified = DateTime.Now.ToString("dd/MM/yyyy");

        }



        public void Print()
        {
            foreach (Tuple<string, string> key in database.Keys)
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

        public Dictionary<Tuple<string, string>, List<string>> Get()
        {
            return this.database;
        }

        public void Set(Dictionary<Tuple<string, string>, List<string>> database)
        {
            this.database = database;
        }
    }
}
