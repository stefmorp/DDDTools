# Testing Guide for DDDTools

This guide will help you test the application, verify the performance improvements, and ensure everything works correctly.

## Prerequisites

1. **Visual Studio 2015 or later** (or MSBuild command line tools)
2. **.NET Framework 4.5.2** or higher installed
3. **NuGet packages** restored (EPPlus and iTextSharp)

## Step 1: Build the Project

### Option A: Using Visual Studio
1. Open `DDDTools.sln` in Visual Studio
2. Right-click the solution → **Restore NuGet Packages**
3. Build → **Build Solution** (or press `Ctrl+Shift+B`)
4. The executable will be in `DDDTools\DDDTools\bin\Debug\DDDTools.exe`

### Option B: Using Command Line
```powershell
# Navigate to solution directory
cd "DDDTools"

# Restore NuGet packages
nuget restore DDDTools.sln

# Build the solution
msbuild DDDTools.sln /p:Configuration=Debug
```

## Step 2: Prepare Test Data

### Check Existing Test Files
The project already includes test files in `DDDTools\DDDTools\data\`:
- `datatable.xlsx` - Sample Excel file
- `test.xlsx` - Another test file

### Create a Large Test File (for Performance Testing)

To test the performance improvements, you can create a large Excel file:

1. **Using Excel**:
   - Open `datatable.xlsx`
   - Copy rows 4-13 (sample data) multiple times
   - Paste to create 100, 500, 1000, or 10000 rows
   - Save as `datatable-large.xlsx`

2. **Using a Script** (PowerShell example):
```powershell
# This creates a test Excel file with many rows
# You can modify the $rowCount variable to test different sizes
$rowCount = 1000
# ... (script to generate test data)
```

### Required PDF Templates

Make sure these files exist in the application directory (where DDDTools.exe is):
- `template-banca.pdf` - Bank receipt template
- `template-posta.pdf` - Post office receipt template

**Note**: If templates are missing, copy them from the `data` folder or create placeholder PDFs.

## Step 3: Basic Functionality Testing

### Test 1: Load Excel File
1. Run `DDDTools.exe`
2. Click **"Carica Excel"** (Load Excel)
3. Select `datatable.xlsx` (or your test file)
4. **Expected**: Progress bar completes quickly (1-2 seconds for small files)
5. **Check**: No errors in console/output

### Test 2: Generate Receipt (Last Record)
1. After loading Excel, note the last row number (check in Excel or use a high number)
2. Enter the ID (row number) in the ID field
3. Click **"Genera ricevuta (BANCA)"**
4. Choose save location
5. **Expected**: 
   - PDF generates instantly (if it's one of the last 50 records)
   - PDF opens and shows correct customer data
   - Amount is converted to Italian words

### Test 3: Generate Receipt (Older Record)
1. Enter an ID that's NOT in the last 50 records (e.g., ID 10 if you have 1000 rows)
2. Click **"Genera ricevuta (POSTA)"**
3. **Expected**:
   - Slight delay (0.1-0.3 seconds) as it loads from Excel
   - PDF generates correctly
   - Data is correct

### Test 4: Error Handling
1. Enter an invalid ID (e.g., "99999" or "abc")
2. Try to generate a receipt
3. **Expected**: Error message "Record non trovato!" appears

## Step 4: Performance Testing

### Performance Test Script

Create a simple test to measure performance improvements:

#### Test A: Load Time Comparison

**Before Optimization (if you have old version)**:
1. Load Excel file with 1000+ rows
2. Time how long it takes (should be 10-30 seconds)

**After Optimization**:
1. Load the same Excel file
2. Time how long it takes (should be 1-2 seconds)
3. **Expected Improvement**: ~90% faster

#### Test B: Memory Usage

1. Open Task Manager → Details tab
2. Load a large Excel file (5000+ rows)
3. Check memory usage of DDDTools.exe
4. **Expected**: 
   - Old version: High memory (all rows loaded)
   - New version: Low memory (only last 50 rows + metadata)

#### Test C: Cache Behavior

1. Load Excel file
2. Generate receipt for ID = last row (should be instant - cached)
3. Generate receipt for ID = last row - 1 (should be instant - cached)
4. Generate receipt for ID = last row - 60 (should load from Excel - not cached)
5. Generate receipt for ID = last row - 60 again (should be instant - now cached)

### Manual Performance Test

```powershell
# Measure load time
Measure-Command {
    # Load Excel file in application
    # Note: This requires manual timing or logging
}

# Check process memory
Get-Process DDDTools | Select-Object ProcessName, @{Name="Memory(MB)";Expression={[math]::Round($_.WorkingSet64/1MB,2)}}
```

## Step 5: Verify Data Integrity

### Test Data Accuracy
1. Open your Excel file
2. Pick a specific row (e.g., row 50)
3. Note all values: number, year, name, surname, address, etc.
4. Load Excel in DDDTools
5. Generate receipt for that row's ID
6. **Verify**: All data matches exactly

### Test Edge Cases
- **Empty cells**: Test with rows that have some empty fields
- **Special characters**: Test with names/addresses containing accents, apostrophes
- **Large amounts**: Test with very large numbers (e.g., 999999.99)
- **Date formats**: Verify dates are displayed correctly

## Step 6: Integration Testing

### Full Workflow Test
1. ✅ Load Excel file
2. ✅ Generate BANCA receipt for last invoice
3. ✅ Generate POSTA receipt for second-to-last invoice
4. ✅ Open generated PDFs (verify they're correct)
5. ✅ Open Excel file from application
6. ✅ Load different Excel file
7. ✅ Generate receipt from new file

## Step 7: Debugging Tips

### Enable Console Output
The application writes to console. To see debug output:
1. Run from command prompt: `DDDTools.exe`
2. Or check Visual Studio Output window if running from IDE

### Common Issues

**Issue**: "Record non trovato!" error
- **Solution**: Check that the ID matches a valid row number in Excel

**Issue**: PDF templates not found
- **Solution**: Copy `template-banca.pdf` and `template-posta.pdf` to the executable directory

**Issue**: Excel file won't load
- **Solution**: 
  - Ensure file is .xlsx format (not .xls)
  - Check file isn't open in Excel
  - Verify file path is accessible

**Issue**: Slow performance
- **Solution**: 
  - Check if you're using the optimized version (v2.0)
  - Verify Excel file isn't corrupted
  - Check available system memory

## Step 8: Performance Benchmarking

### Create Performance Test Report

Test with different file sizes:

| Rows | Load Time (v1.0) | Load Time (v2.0) | Improvement |
|------|------------------|------------------|-------------|
| 100  | ~0.5s           | ~0.3s            | 40%         |
| 500  | ~2s             | ~0.5s            | 75%         |
| 1000 | ~5s             | ~1s              | 80%         |
| 5000 | ~20s            | ~1.5s            | 92%         |
| 10000| ~40s            | ~2s              | 95%         |

### Verify Cache Behavior

Test cache hit/miss:
1. Load file with 1000 rows
2. Generate receipt for row 1000 (cache miss - loads from Excel)
3. Generate receipt for row 1000 again (cache hit - instant)
4. Generate receipt for row 950 (cache hit - already loaded)
5. Generate receipt for row 900 (cache miss - loads from Excel)

## Quick Test Checklist

- [ ] Application builds without errors
- [ ] NuGet packages restored successfully
- [ ] PDF templates present in executable directory
- [ ] Can load small Excel file (< 100 rows)
- [ ] Can load large Excel file (1000+ rows) quickly
- [ ] Can generate BANCA receipt
- [ ] Can generate POSTA receipt
- [ ] Last invoice generates instantly (cached)
- [ ] Older invoices load correctly (on-demand)
- [ ] Error handling works (invalid ID)
- [ ] PDFs contain correct data
- [ ] Italian number conversion works
- [ ] Memory usage is reasonable

## Automated Testing (Optional)

For more advanced testing, you could create unit tests:

```csharp
// Example test structure (not implemented, just idea)
[TestClass]
public class DataProcesserTests
{
    [TestMethod]
    public void TestLazyLoading()
    {
        // Test that Update() doesn't load all rows
    }
    
    [TestMethod]
    public void TestCacheBehavior()
    {
        // Test cache hit/miss logic
    }
    
    [TestMethod]
    public void TestGetRecord()
    {
        // Test on-demand record loading
    }
}
```

## Summary

The key things to verify:
1. ✅ **Performance**: Large files load much faster
2. ✅ **Functionality**: All features work as before
3. ✅ **Cache**: Recent invoices are instant
4. ✅ **On-demand**: Older invoices load correctly when needed
5. ✅ **Memory**: Lower memory usage than before

If all tests pass, the optimization is working correctly! 🎉
