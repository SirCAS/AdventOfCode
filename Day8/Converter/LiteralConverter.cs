using System;
using System.Collections.Generic;
using System.Linq;
using Day8.Model;

namespace Day8.Converter
{
    public class LiteralConverter
    {
        private IList<StringContainer> dataSet;

        public LiteralConverter(string[] input)
        {
            dataSet = input
                .Select(x => x.Substring(1, x.Length - 2))
                .SelectMany(x => x)
                .ToArray().Select(x => new StringContainer
                {
                    Value = x,
                    State = ValueState.None
                })
                .ToList();
        }

        public void FixHexaDecimals()
        {
            for (int x = 0; x < dataSet.Count - 4; ++x)
            {
                if (dataSet[x].Value == '\\' && dataSet[x].State == ValueState.None &&
                   dataSet[x + 1].Value == 'x' && dataSet[x + 1].State == ValueState.None)
                {
                    var hexValues = char.ToString(dataSet[x + 2].Value) + char.ToString(dataSet[x + 3].Value);

                    // Now validate if can make the conversion

                    try
                    {
                        uint hexUint = Convert.ToUInt32(hexValues, 16);
                        char hexAscii = Convert.ToChar(hexUint);

                        dataSet[x].Value = hexAscii;
                        dataSet[x].State = ValueState.Modified;
                        dataSet[x + 1].State = ValueState.Deleted;
                        dataSet[x + 2].State = ValueState.Deleted;
                        dataSet[x + 3].State = ValueState.Deleted;
                    }
                    catch (FormatException)
                    {
                        // Supress exception
                    }
                }
            }
        }

        public void FixQuotes()
        {
            for (int x = 0; x < dataSet.Count - 1; ++x)
            {
                if (
                    dataSet[x].Value == '\\' &&
                    dataSet[x].State == ValueState.None &&
                    dataSet[x + 1].Value == '"' &&
                    dataSet[x + 1].State == ValueState.None
                    )
                {
                    dataSet[x].Value = '"';
                    dataSet[x].State = ValueState.Modified;
                    dataSet[x + 1].State = ValueState.Deleted;
                }
            }
        }

        public void FixBackslash()
        {
            for (int x = 0; x < dataSet.Count - 1; ++x)
            {
                if (
                    dataSet[x].Value == '\\' &&
                    dataSet[x].State == ValueState.None &&
                    dataSet[x + 1].Value == '\\' &&
                    dataSet[x + 1].State == ValueState.None
                    )
                {
                    dataSet[x].Value = '\\';
                    dataSet[x].State = ValueState.Modified;
                    dataSet[x + 1].State = ValueState.Deleted;
                }
            }
        }

        public string GetResult()
        {
            var result = dataSet.Where(x => x.State == ValueState.None || x.State == ValueState.Modified).Select(x => x.Value).ToArray();
            return new string(result);
        }
    }
}
