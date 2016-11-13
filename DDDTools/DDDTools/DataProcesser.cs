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

        private Dictionary<string, List<string>> database = new Dictionary<string, List<string>>();
        private string lastmodified = "22/06/2016";
        private int lastID = 0;
        private string lastnumber = "0000";
        //private string _path;
        private FileInfo datatable;


        //public DataProcesser(string path)
        //{
        //    _path = path;
        //    FileInfo datatable = new FileInfo(_path);
        //}




        public void Update(String path)
        {
            using (ExcelPackage xlPackage = new ExcelPackage(datatable = new FileInfo(path))) 
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];


                for (int row = 1; row <= 584; row++)
                {

                    Store(worksheet.Cell(row, 1).Value, worksheet.Cell(row, 2).Value, worksheet.Cell(row, 3).Value, worksheet.Cell(row, 4).Value,
                        worksheet.Cell(row, 5).Value, worksheet.Cell(row, 6).Value, worksheet.Cell(row, 7).Value, worksheet.Cell(row, 8).Value,
                        worksheet.Cell(row, 9).Value, worksheet.Cell(row, 10).Value, worksheet.Cell(row, 11).Value, worksheet.Cell(row, 12).Value, worksheet.Cell(row, 13).Value);

                }
                    //for (int col = 1; col < 13; col++)
                    //{
                    // Console.WriteLine("Cell({0},{1}).Value={2}", row, col, worksheet.Cell(row, col).Value);
                    //}
                


            } // the using statement calls Dispose() which closes the package.
        }

        // the fullname (a tuple containing name and surname), the number of the transaction,... (explaines itself) ... IVA is 'partita iva', amount is in Euro and the date of the transaction
        public void Store(string id, string number, string year ,string name, string surname, string address, string cap, string city, string province, string fiscalcode, string IVA, string amount, string date)
        {
            if (!(database.ContainsKey(id))){
                database.Add(id, new List<string> { number, year, name, surname, address, cap, city, province, fiscalcode, IVA, amount, date });
                lastmodified = DateTime.Now.ToString("dd/MM/yyyy");
                lastID++;
                lastnumber = number;
            }
            else
            {
                Console.Write("Already Contains key");
            }
           
        }

        public void Write(string id, string number, string year, string name, string surname, string address, string cap, string city, string province, string fiscalcode, string IVA, string amount, string date)
        {

            using (ExcelPackage xlPackage = new ExcelPackage(datatable))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];

                worksheet.Cell(lastID, 1).Value = id;      
                worksheet.Cell(lastID, 2).Value = number;
                worksheet.Cell(lastID, 3).Value = year;
                worksheet.Cell(lastID, 4).Value = name;
                worksheet.Cell(lastID, 5).Value = surname;
                worksheet.Cell(lastID, 6).Value = address;
                worksheet.Cell(lastID, 7).Value = cap;
                worksheet.Cell(lastID, 8).Value = city;
                worksheet.Cell(lastID, 9).Value = province;
                worksheet.Cell(lastID, 10).Value = fiscalcode;
                worksheet.Cell(lastID, 11).Value = IVA;
                worksheet.Cell(lastID, 12).Value = amount;
                worksheet.Cell(lastID, 13).Value = date;

                xlPackage.Save();
            }

            

        }



        public void Print()
        {
            foreach (string key in database.Keys)
            {
                Console.Write(key);

                foreach (string s in database[key])
                {
                    Console.Write(" " + s);
                }
                Console.Write("\n");

            }
            Console.WriteLine("Lastmodified: " + lastmodified);
        }

        public Dictionary<string, List<string>> Get()
        {
            return this.database;
        }

        public void Set(Dictionary<string, List<string>> database)
        {
            this.database = database;
        }

        public int getLastId()
        {
            return lastID;
        }
        public void setLastId()
        {
            lastID++;
        }
        public string getLastNumber()
        {
            return lastnumber;
        }

    }
}
