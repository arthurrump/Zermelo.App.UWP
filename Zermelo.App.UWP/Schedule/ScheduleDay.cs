using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace Zermelo.App.UWP.Schedule
{
    public class ScheduleDay : IEquatable<ScheduleDay>
    {
        public ScheduleDay(LocalDate date, ObservableCollection<ScheduleRow> appointments)
        {
            Date = date;
            Appointments = appointments;
        }

        public LocalDate Date { get; set; }
        public ObservableCollection<ScheduleRow> Appointments { get; set; }

        public override int GetHashCode()
            => Date.GetHashCode();

        public bool Equals(ScheduleDay other)
            => (Date == other.Date && Appointments.SequenceEqual(other.Appointments));
    }
}
