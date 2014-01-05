using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZBusDocConversion
{
    public static class Extensions
    {
        public static string RemoveWordAndTrim(this string text, string wordToRemove)
        {
            return text.Replace(wordToRemove, "").Trim();
        }
    }
}
