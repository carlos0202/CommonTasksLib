using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Dynamic;
using CommonTasksLib.Collections;

namespace CommonTasksLib.Data.ADOExtensions
{
    public static class ADOHelperExtensions
    {
        public static List<ExpandoObject> ToExpandoArray<T>(this T source, string includedFields = null, string excludedFields = null) 
            where T: DbDataReader
        {
            List<ExpandoObject> result = new List<ExpandoObject>();
            List<string> ToInclude = includedFields == null ?
                new List<string>() : includedFields.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries).ToList();
            ToInclude.ForEach(i => i = i.Trim());
            List<string> ToExclude = excludedFields == null ?
                new List<String>() : excludedFields.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            ToExclude.ForEach(i => i = i.Trim());
            source.Select(r => r).AsParallel().ForAll(row =>
            {
                ExpandoObject tmpRow = new ExpandoObject();
                var DataColumn = (ICollection<KeyValuePair<string, object>>)tmpRow;
                for (int i = 0; i < row.FieldCount; i++)
                {
                    if (ToExclude.Contains(row.GetName(i), StringComparer.InvariantCultureIgnoreCase) && 
                        !ToInclude.Contains(row.GetName(i), StringComparer.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    else
                    {
                        DataColumn.Add(new KeyValuePair<string, object>(row.GetName(i), row.GetValue(i)));
                    }
                }
                result.Add(tmpRow);
            });

            return result;
        }

    }
}

