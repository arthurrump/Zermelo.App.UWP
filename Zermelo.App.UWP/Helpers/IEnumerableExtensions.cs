using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zermelo.App.UWP.Helpers
{
    static class IEnumerableExtensions
    {
        public static string ToCommaSpaceSeperatedString<T>(this IEnumerable<T> list)
        {
            if (list.Count() < 1) return "";

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
