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
        private string datatablePath = "";
        private int totalRows = 0; // Cache the total number of rows
        private const int CACHE_SIZE = 50; // Cache last 50 records for quick access


        //public DataProcesser(string path)
        //{
        //    _path = path;
        //    FileInfo datatable = new FileInfo(_path);
        //}




        public void Update(String path)
        {
            datatablePath = path;
            datatable = new FileInfo(path);
            
            // Only scan to find the last row and metadata - don't load all data
            using (ExcelPackage xlPackage = new ExcelPackage(datatable)) 
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                
                // Find the last row efficiently by scanning backwards
                int lastRow = 4;
                if (worksheet.Dimension != null)
                {
                    lastRow = worksheet.Dimension.End.Row;
                    
                    // Scan backwards to find the actual last non-empty row (more efficient)
                    for (int row = lastRow; row >= 4; row--)
                    {
                        var cellValue = worksheet.Cell(row, 1).Value;
                        if (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
                        {
                            lastRow = row;
                            break;
                        }
                    }
                }
                
                totalRows = lastRow;
                
                // Only load metadata from the last row
                if (lastRow >= 4)
                {
                    var lastRowData = worksheet.Cell(lastRow, 1).Value;
                    if (lastRowData != null)
                    {
                        lastID = lastRow;
                        lastnumber = worksheet.Cell(lastRow, 2).Value?.ToString() ?? "0000";
                    }
                }
                
                // Clear old cache and load only the last CACHE_SIZE records for quick access
                database.Clear();
                LoadLastRecords(CACHE_SIZE);
            }
        }
        
        // Load only the last N records for quick access
        private void LoadLastRecords(int count)
        {
            if (datatable == null || !datatable.Exists) return;
            
            using (ExcelPackage xlPackage = new ExcelPackage(datatable))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                
                int startRow = Math.Max(4, totalRows - count + 1);
                
                for (int row = startRow; row <= totalRows; row++)
                {
                    var cellValue = worksheet.Cell(row, 1).Value;
                    if (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
                    {
                        LoadRecord(row, worksheet);
                    }
                }
            }
        }
        
        // Load a specific record by row number
        private void LoadRecord(int row, ExcelWorksheet worksheet)
        {
            string id = row.ToString();
            if (!database.ContainsKey(id))
            {
                database.Add(id, new List<string> 
                { 
                    worksheet.Cell(row, 2).Value?.ToString() ?? "",  // number
                    worksheet.Cell(row, 3).Value?.ToString() ?? "",  // year
                    worksheet.Cell(row, 4).Value?.ToString() ?? "",  // name
                    worksheet.Cell(row, 5).Value?.ToString() ?? "",  // surname
                    worksheet.Cell(row, 6).Value?.ToString() ?? "",  // address
                    worksheet.Cell(row, 7).Value?.ToString() ?? "",  // cap
                    worksheet.Cell(row, 8).Value?.ToString() ?? "",  // city
                    worksheet.Cell(row, 9).Value?.ToString() ?? "",  // province
                    worksheet.Cell(row, 10).Value?.ToString() ?? "", // fiscalcode
                    worksheet.Cell(row, 11).Value?.ToString() ?? "", // IVA
                    worksheet.Cell(row, 12).Value?.ToString() ?? "", // amount
                    worksheet.Cell(row, 13).Value?.ToString() ?? ""  // date
                });
            }
        }
        
        // Load a specific record by ID on-demand
        public List<string> GetRecord(string id)
        {
            // Check cache first
            if (database.ContainsKey(id))
            {
                return database[id];
            }
            
            // Load from Excel if not in cache
            if (datatable != null && datatable.Exists)
            {
                using (ExcelPackage xlPackage = new ExcelPackage(datatable))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                    int row = int.Parse(id);
                    LoadRecord(row, worksheet);
                    
                    // If cache is getting too large, remove oldest entries
                    if (database.Count > CACHE_SIZE * 2)
                    {
                        var keysToRemove = database.Keys
                            .Select(k => int.Parse(k))
                            .OrderBy(k => k)
                            .Take(database.Count - CACHE_SIZE)
                            .Select(k => k.ToString())
                            .ToList();
                        
                        foreach (var key in keysToRemove)
                        {
                            database.Remove(key);
                        }
                    }
                    
                    return database.ContainsKey(id) ? database[id] : null;
                }
            }
            
            return null;
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
            if (datatable == null || !datatable.Exists) return;

            using (ExcelPackage xlPackage = new ExcelPackage(datatable))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                
                int row = int.Parse(id);

                worksheet.Cell(row, 1).Value = id;      
                worksheet.Cell(row, 2).Value = number;
                worksheet.Cell(row, 3).Value = year;
                worksheet.Cell(row, 4).Value = name;
                worksheet.Cell(row, 5).Value = surname;
                worksheet.Cell(row, 6).Value = address;
                worksheet.Cell(row, 7).Value = cap;
                worksheet.Cell(row, 8).Value = city;
                worksheet.Cell(row, 9).Value = province;
                worksheet.Cell(row, 10).Value = fiscalcode;
                worksheet.Cell(row, 11).Value = IVA;
                worksheet.Cell(row, 12).Value = amount;
                worksheet.Cell(row, 13).Value = date;
                
                // Update cache if this record is cached
                if (database.ContainsKey(id))
                {
                    database[id] = new List<string> { number, year, name, surname, address, cap, city, province, fiscalcode, IVA, amount, date };
                }
                
                // Update metadata
                if (row > lastID)
                {
                    lastID = row;
                    totalRows = row;
                    lastnumber = number;
                }

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
        
        // Optimized Get method that loads records on-demand
        public List<string> GetById(string id)
        {
            return GetRecord(id);
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
