using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Zermelo.App.UWP.Schedule
{
    class CurrentDateToSelectedPivotIndexConverter : DependencyObject, IValueConverter
    {
        public DateInterval CurrentWeek
        {
            get { return (DateInterval)GetValue(CurrentWeekProperty); }
            set { SetValue(CurrentWeekProperty, value); }
        }
        public static readonly DependencyProperty CurrentWeekProperty =
            DependencyProperty.Register(nameof(CurrentWeek), typeof(DateInterval), typeof(CurrentDateToSelectedPivotIndexConverter), new PropertyMetadata(null));
        
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is LocalDate date && targetType == typeof(int))
                return date.DayOfWeek - 1;

            throw new InvalidOperationException("Converts only from LocalDate to int.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is int index && targetType == typeof(LocalDate))
                return CurrentWeek.Start.PlusDays(index);

            throw new InvalidOperationException("Converts only from int to LocalDate.");
        }
    }
}
