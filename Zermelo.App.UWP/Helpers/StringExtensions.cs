using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zermelo.App.UWP.Helpers
{
    public static class StringExtensions
    {
        public static string MaxLength(this string s, int maxLength)
            => s.Length <= maxLength ? s : s.Substring(0, maxLength);
    }
}
