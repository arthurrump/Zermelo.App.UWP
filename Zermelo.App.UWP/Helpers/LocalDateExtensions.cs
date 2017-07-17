using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using NodaTime.Extensions;

namespace Zermelo.App.UWP.Helpers
{
    public static class LocalDateExtensions
    {
        public static DateTimeOffset ToMidnightDateTimeOffset(this LocalDate date)
            => date.AtMidnight().WithOffset(DateTimeOffset.Now.Offset.ToOffset()).ToDateTimeOffset();

        public static string ToMidnightUnixTimeString(this LocalDate date)
            => date.ToMidnightDateTimeOffset().ToUnixTimeSeconds().ToString();
    }
}
