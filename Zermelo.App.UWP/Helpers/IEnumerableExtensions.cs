using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zermelo.App.UWP.Helpers
{
    static class IEnumerableExtensions
    {
        public static string ToCommaSpaceSeperatedString<T>(this IEnumerable<T> list)
        {
            StringBuilder s = new StringBuilder();
            
            foreach (T t in list)
            {
                s.Append(t.ToString());
                s.Append(", ");
            }

            s.Remove(s.Length - 2, 2);

            return s.ToString();
        }
    }
}
