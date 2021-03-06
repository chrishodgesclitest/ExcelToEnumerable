using System;

namespace ExcelToEnumerable.Attributes
{
    /// <summary>
    /// Maps the class to a spreadsheet in the workbook with the specific name.
    /// </summary>
    public class UsingSheetAttribute : Attribute
    {
        /// <summary>
        /// Maps the class to the spreadsheet with the given name
        /// </summary>
        /// <param name="column"></param>
        /// <exception cref="NotImplementedException"></exception>
        // ReSharper disable once UnusedParameter.Local
        public UsingSheetAttribute(string column)
        {
        }
        
        /// <summary>
        /// Maps the class to the spreadsheet at the given zero-based index
        /// </summary>
        /// <param name="column"></param>
        public UsingSheetAttribute(int column)
        {
        }
    }
}