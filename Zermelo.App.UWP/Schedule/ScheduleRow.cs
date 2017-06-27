using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace Zermelo.App.UWP.Schedule
{
    public class ScheduleRow : IEquatable<ScheduleRow>
    {
        public ScheduleRow(DateTimeOffset start, IEnumerable<Appointment> items)
        {
            Start = start;
            Items = items;
        }

        public DateTimeOffset Start { get; }
        public IEnumerable<Appointment> Items { get; }

        public override int GetHashCode()
            => Start.GetHashCode();

        public bool Equals(ScheduleRow other)
            => (Start == other.Start && Items.SequenceEqual(other.Items));
    }
}
