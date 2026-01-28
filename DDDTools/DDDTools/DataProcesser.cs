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
        
        // Thread synchronization lock object for protecting dictionary and metadata access
        private readonly object _lockObject = new object();


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
                        var cellValue = worksheet.Cells[row, 1].Value;
                        if (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
                        {
                            lastRow = row;
                            break;
                        }
                    }
                }
                
                // Clear old cache and load only the last CACHE_SIZE records for quick access
                // Thread-safe: lock to prevent race conditions with GetRecord/GetById
                lock (_lockObject)
                {
                    // Update metadata atomically with cache operations
                    totalRows = lastRow;
                    
                    // Only load metadata from the last row
                    if (lastRow >= 4)
                    {
                        var lastRowData = worksheet.Cells[lastRow, 1].Value;
                        if (lastRowData != null)
                        {
                            lastID = lastRow;
                            lastnumber = worksheet.Cells[lastRow, 1].Value?.ToString() ?? "0000";
                        }
                    }
                    
                    database.Clear();
                    LoadLastRecords(CACHE_SIZE);
                }
            }
        }
        
        // Load only the last N records for quick access
        // Thread-safe: Must be called from within a lock(_lockObject) block
        private void LoadLastRecords(int count)
        {
            if (datatable == null || !datatable.Exists) return;
            
            using (ExcelPackage xlPackage = new ExcelPackage(datatable))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                
                int startRow = Math.Max(4, totalRows - count + 1);
                
                for (int row = startRow; row <= totalRows; row++)
                {
                    var cellValue = worksheet.Cells[row, 1].Value;
                    if (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
                    {
                        LoadRecord(row, worksheet);
                    }
                }
            }
        }
        
        // Load a specific record by row number
        // Note: The 'row' parameter is the Excel row number (e.g., 4, 5, 6, etc.)
        // The ID used in the application is derived from this row number (id = row.ToString())
        // Data is stored in columns 1-12 (no ID column exists in Excel)
        // Thread-safe: must be called within a lock(_lockObject) block
        private void LoadRecord(int row, ExcelWorksheet worksheet)
        {
            string id = row.ToString();
            if (!database.ContainsKey(id))
            {
                database.Add(id, new List<string> 
                { 
                    worksheet.Cells[row, 1].Value?.ToString() ?? "",  // number
                    worksheet.Cells[row, 2].Value?.ToString() ?? "",  // year
                    worksheet.Cells[row, 3].Value?.ToString() ?? "",  // name
                    worksheet.Cells[row, 4].Value?.ToString() ?? "",  // surname
                    worksheet.Cells[row, 5].Value?.ToString() ?? "",  // address
                    worksheet.Cells[row, 6].Value?.ToString() ?? "",  // cap
                    worksheet.Cells[row, 7].Value?.ToString() ?? "",  // city
                    worksheet.Cells[row, 8].Value?.ToString() ?? "",  // province
                    worksheet.Cells[row, 9].Value?.ToString() ?? "",  // fiscalcode
                    worksheet.Cells[row, 10].Value?.ToString() ?? "", // IVA
                    worksheet.Cells[row, 11].Value?.ToString() ?? "", // amount
                    worksheet.Cells[row, 12].Value?.ToString() ?? ""  // date
                });
            }
        }
        
        // Load a specific record by ID on-demand
        // Note: 'id' is the Excel row number (e.g., "4" for row 4, "5" for row 5, etc.)
        // The ID is NOT stored in the Excel file - it's derived from the row position
        // Thread-safe: all dictionary access is protected with locks
        public List<string> GetRecord(string id)
        {
            // Check cache first (thread-safe)
            lock (_lockObject)
            {
                if (database.ContainsKey(id))
                {
                    return database[id];
                }
            }
            
            // Load from Excel if not in cache
            if (datatable != null && datatable.Exists)
            {
                try
                {
                    int row = int.Parse(id);
                    
                    using (ExcelPackage xlPackage = new ExcelPackage(datatable))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                        
                        // Load record and manage cache (thread-safe)
                        lock (_lockObject)
                        {
                            LoadRecord(row, worksheet);
                            
                            // If cache is getting too large, remove oldest entries
                            if (database.Count > CACHE_SIZE * 2)
                            {
                                var keysToRemove = new List<string>();
                                foreach (var key in database.Keys)
                                {
                                    try
                                    {
                                        int.Parse(key); // Valid key, keep for now
                                    }
                                    catch
                                    {
                                        // Invalid key format, remove it
                                        keysToRemove.Add(key);
                                    }
                                }
                                
                                // Remove invalid keys first
                                foreach (var key in keysToRemove)
                                {
                                    database.Remove(key);
                                }
                                
                                // If still too large, remove oldest numeric entries
                                if (database.Count > CACHE_SIZE * 2)
                                {
                                    var numericKeys = database.Keys
                                        .Where(k => { try { int.Parse(k); return true; } catch { return false; } })
                                        .Select(k => int.Parse(k))
                                        .OrderBy(k => k)
                                        .Take(database.Count - CACHE_SIZE)
                                        .Select(k => k.ToString())
                                        .ToList();
                                    
                                    foreach (var key in numericKeys)
                                    {
                                        database.Remove(key);
                                    }
                                }
                            }
                            
                            return database.ContainsKey(id) ? database[id] : null;
                        }
                    }
                }
                catch (FormatException)
                {
                    // Invalid ID format - return null instead of crashing
                    return null;
                }
                catch (OverflowException)
                {
                    // ID too large - return null instead of crashing
                    return null;
                }
            }
            
            return null;
        }

        // the fullname (a tuple containing name and surname), the number of the transaction,... (explaines itself) ... IVA is 'partita iva', amount is in Euro and the date of the transaction
        // Thread-safe: all dictionary and metadata access is protected with locks
        public void Store(string id, string number, string year ,string name, string surname, string address, string cap, string city, string province, string fiscalcode, string IVA, string amount, string date)
        {
            lock (_lockObject)
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
           
        }

        // Write a record to the Excel file
        // Note: 'id' is the Excel row number (e.g., "4" for row 4, "5" for row 5, etc.)
        // The ID is NOT written to the Excel file - it's only used to determine which row to write to
        public void Write(string id, string number, string year, string name, string surname, string address, string cap, string city, string province, string fiscalcode, string IVA, string amount, string date)
        {
            if (datatable == null || !datatable.Exists) return;

            try
            {
                int row = int.Parse(id);
                
                using (ExcelPackage xlPackage = new ExcelPackage(datatable))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];

                    // Write data to columns 1-12 (matching LoadRecord and original format)
                    // Note: ID is not stored - it's the row number, which is redundant
                    worksheet.Cells[row, 1].Value = number;
                    worksheet.Cells[row, 2].Value = year;
                    worksheet.Cells[row, 3].Value = name;
                    worksheet.Cells[row, 4].Value = surname;
                    worksheet.Cells[row, 5].Value = address;
                    worksheet.Cells[row, 6].Value = cap;
                    worksheet.Cells[row, 7].Value = city;
                    worksheet.Cells[row, 8].Value = province;
                    worksheet.Cells[row, 9].Value = fiscalcode;
                    worksheet.Cells[row, 10].Value = IVA;
                    worksheet.Cells[row, 11].Value = amount;
                    worksheet.Cells[row, 12].Value = date;
                    
                    // Update cache and metadata (thread-safe)
                    lock (_lockObject)
                    {
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
                    }

                    xlPackage.Save();
                }
            }
            catch (FormatException)
            {
                // Invalid ID format - throw a more descriptive exception or log error
                throw new ArgumentException($"Invalid ID format: '{id}'. ID must be a valid integer.", nameof(id));
            }
            catch (OverflowException)
            {
                // ID too large - throw a more descriptive exception
                throw new ArgumentException($"ID value '{id}' is too large. ID must be within integer range.", nameof(id));
            }
        }



        // Thread-safe: dictionary iteration is protected with lock
        public void Print()
        {
            lock (_lockObject)
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
        }

        // Thread-safe: returns a copy of the dictionary to avoid exposing internal state
        public Dictionary<string, List<string>> Get()
        {
            lock (_lockObject)
            {
                // Return a copy to prevent external modification and thread safety issues
                return new Dictionary<string, List<string>>(this.database);
            }
        }
        
        // Optimized Get method that loads records on-demand
        // Note: 'id' is the Excel row number (e.g., "4" for row 4, "5" for row 5, etc.)
        // The ID is NOT stored in the Excel file - it's derived from the row position
        public List<string> GetById(string id)
        {
            return GetRecord(id);
        }

        // Thread-safe: replaces the entire dictionary atomically
        public void Set(Dictionary<string, List<string>> database)
        {
            lock (_lockObject)
            {
                this.database = database;
            }
        }

        // Thread-safe: metadata access is protected
        public int getLastId()
        {
            lock (_lockObject)
            {
                return lastID;
            }
        }
        
        // Thread-safe: metadata modification is protected
        public void setLastId()
        {
            lock (_lockObject)
            {
                lastID++;
            }
        }
        
        // Thread-safe: metadata access is protected
        public string getLastNumber()
        {
            lock (_lockObject)
            {
                return lastnumber;
            }
        }

    }
}
