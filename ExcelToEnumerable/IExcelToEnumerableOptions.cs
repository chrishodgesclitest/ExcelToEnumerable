using System;
using System.Collections.Generic;

namespace ExcelToEnumerable
{
    internal interface IExcelToEnumerableOptions<T>
    {
        BlankRowBehaviour BlankRowBehaviour { get; }
        
        IList<Exception> ExceptionList { get; }
        
        IList<string> RequiredFields { get; }
        
        int? WorksheetNumber { get; }
        
        string WorksheetName { get; }
        
        bool UseHeaderNames { get; }
        
        int? StartRow { get; }
        
        int HeaderRow { get; }
        
        int? EndRow { get; }
        
        ExceptionHandlingBehaviour ExceptionHandlingBehaviour { get; }
        
        Dictionary<string, List<ExcelCellValidator>> Validations { get; set; }
        
        IEnumerable<string> LoweredRequiredColumns { get; }
        
        Dictionary<string, ExcelToEnumerableCollectionConfiguration> CollectionConfigurations { get; set; }
        
        List<string> UniqueProperties { get; set; }
        
        Action<IDictionary<int, string>> OnReadingHeaderRowAction { get; }
        
        Dictionary<string, Func<object, object>> CustomMappings { get; }
        
        Dictionary<string, string> CustomHeaderNames { get; }
        
        List<string> UnmappedProperties { get; set; }
        
        string RowNumberProperty { get; set; }
        
        string LoweredRowNumberProperty { get; }
        
        Dictionary<string, int> CustomHeaderNumbers { get; }
        
        List<string> OptionalProperties { get; set; }
        
        bool IgnoreColumnsWithoutMatchingProperties { get; set; }
        
        List<string> ExplicitlyRequiredProperties { get; set; }
        
        bool AllPropertiesOptionalByDefault { get; set; }
    }
}