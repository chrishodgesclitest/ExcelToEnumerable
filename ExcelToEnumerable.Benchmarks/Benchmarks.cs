using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using ExcelDataReader;
using Ganss.Excel;

namespace ExcelToEnumerable.Benchmarks
{
    public class Benchmarks
    {
        private string _complexExample;
        private string _filePath;
        private string _hugeFilePath;

        private string GetTestSpreadsheet(string spreadsheetName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyPath = Path.GetDirectoryName(assembly.GetName().CodeBase).Substring(5);
            assemblyPath =
                Regex.Replace(assemblyPath, @"^\\+(?<drive>[A-Z]:)", "${drive}"); //Fix for windows based file systems
            var testSpreadsheetLocation = Path.Combine(assemblyPath, "TestSpreadsheets", spreadsheetName);
            return testSpreadsheetLocation;
        }

        [GlobalSetup]
        public void Setup()
        {
            _filePath = GetTestSpreadsheet("TestSpreadsheet2.xlsx");
            _hugeFilePath = GetTestSpreadsheet("TestSpreadsheet3.xlsx");
            _complexExample = GetTestSpreadsheet("ComplexValidation.xlsx");
        }

        //[Benchmark]
        public void ComplexExample()
        {
            var exceptionList = new List<Exception>();
            var list = _complexExample.ExcelToEnumerable<ExcelQuoteSheetRow>(x => x.StartingFromRow(2)
                .UsingHeaderNames(true)
                .UsingSheet("Prices")
                .OutputExceptionsTo(exceptionList)
                .Property(y => y.Vat).NotNull()
                .Property(y => y.Vat).ShouldBeOneOf("Standard", "Reduced", "2nd Reduced", "Zero")
                .Property(y => y.Measure).NotNull()
                .Property(y => y.Measure).ShouldBeOneOf("g", "kg", "Each", "lt")
                .Property(y => y.Sku).ShouldBeUnique()
                .Property(y => y.Sku).NotNull()
                .Property(y => y.TranslatedSupplierDescriptions).NotNull()
                .Property(y => y.TranslatedSupplierDescriptions).MapFromColumns("Supplier Description")
                .Property(y => y.Price).NotNull()
                .Property(y => y.Price).ShouldBeGreaterThan(0)
                .Property(y => y.Unit).NotNull()
                .Property(y => y.Unit).ShouldBeGreaterThan(0)
                .Property(y => y.DepotExclusions).MapFromColumns("Reynolds Dairy", "Waltham Cross"));
        }

        //[Benchmark]
        public void HugeFile()
        {
            _hugeFilePath.ExcelToEnumerable<TestClass2>(x => x.StartingFromRow(2));
        }

        [Benchmark]
        public void CurrentVersion()
        {
            var result = _filePath.ExcelToEnumerable<TestClass2>(x => x.StartingFromRow(3));
            if (!result.Any())
            {
                throw new Exception("No results");
            }
        }

        [Benchmark]
        public void ExcelEntityMapper()
        {
            var testClasses = new ExcelMapper(_filePath).Fetch<TestClass2>();
        }

        [Benchmark]
        public List<TestClass2> ExcelDataReader()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var list = new List<TestClass2>();
            using (var stream = File.Open(_filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                    reader.Read();
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            list.Add(new TestClass2(reader));
                        }
                    }
                }
            }

            return list;
        }
    }
}