using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace Zermelo.App.UWP.Schedule
{
    public class ScheduleViewState
    {
        public LocalDate CurrentDate { get; set; }
        public ScheduleType Type { get; set; }
        public Appointment SelectedAppointment { get; set; }
        public bool IsModal { get; set; }
    }
}
