using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Zermelo.App.UWP.Schedule
{
    public class StatusToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is AppointmentStatus status && targetType == typeof(Symbol))
            {
                switch (status)
                {
                    case AppointmentStatus.Cancelled:
                        return Symbol.Cancel;
                    case AppointmentStatus.MovedValid:
                    case AppointmentStatus.MovedInvalid:
                    case AppointmentStatus.Invalid:
                        return Symbol.Important;
                    case AppointmentStatus.New:
                        return Symbol.Add;
                    case AppointmentStatus.Normal:
                    default:
                        return 0;
                }
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) 
            => throw new NotImplementedException();
    }
}
