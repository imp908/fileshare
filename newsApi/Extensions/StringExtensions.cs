using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAPI.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsAny(this string str, params string[] values)
        {
            if (!String.IsNullOrEmpty(str) || values.Length > 0)
            {
                foreach (string item in values)
                {
                    if (str.Contains(item))
                    { return true; }
                }
            }
            return false;
        }
    }
}