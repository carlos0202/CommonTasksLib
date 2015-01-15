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
        public static List<ExpandoObject> ToExpandoArray<T>(this T source) where T: DbDataReader
        {
            List<ExpandoObject> result = new List<ExpandoObject>();
            source.Select(r => r).AsParallel().ForAll(row =>
            {
                ExpandoObject tmpRow = new ExpandoObject();
                var DataColumn = (ICollection<KeyValuePair<string, object>>)tmpRow;
                for (int i = 0; i < row.FieldCount; i++)
                {
                    DataColumn.Add(new KeyValuePair<string, object>(row.GetName(i), row.GetValue(i)));
                }
                result.Add(tmpRow);
            });

            return result;
        }

    }
}
