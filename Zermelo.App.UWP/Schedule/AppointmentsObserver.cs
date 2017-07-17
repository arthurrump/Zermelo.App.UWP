using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using Windows.UI.Popups;
using Zermelo.App.UWP.Helpers;

namespace Zermelo.App.UWP.Schedule
{
    public class AppointmentsObserver : IObserver<IEnumerable<Appointment>>
    {
        ObservableCollection<ScheduleRow> _appointments;
        public AppointmentsObserver(ObservableCollection<ScheduleRow> appointments)
        {
            _appointments = appointments;
        }

        public void OnNext(IEnumerable<Appointment> value)
        {
            _appointments.MorphInto(value.GroupBy(x => x.Start).OrderBy(x => x.Key)
                                         .Select(x => new ScheduleRow(x.Key, x.OrderBy(y => y.Status)))
            );
        }

        public void OnError(Exception error)
            => ExceptionHelper.HandleException(error, nameof(ScheduleViewModel), m => new MessageDialog(m, "Error").ShowAsync());

        public void OnCompleted() { }
    }
}
