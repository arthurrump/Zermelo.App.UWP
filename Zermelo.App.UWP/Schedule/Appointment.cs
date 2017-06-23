using System;
using System.Collections.Generic;
using Zermelo.App.UWP.Helpers;

namespace Zermelo.App.UWP.Schedule
{
    public enum AppointmentStatus
    {
        Normal,
        Cancelled,
        MovedValid,
        MovedInvalid,
        New,
        Invalid
    }

    public class Appointment : IEquatable<Appointment>
    {
        public static List<string> Fields => new List<string> {
            "cancelled", "changeDescription", "end", "groups", "locations", "modified",
            "moved", "new", "remark", "start", "startTimeSlot", "subjects", "teachers",
            "valid"
        };

        public Appointment() { } // For JSON serialization

        public Appointment(API.Models.Appointment a)
        {
            Subjects = a.Subjects.ToCommaSpaceSeperatedString();
            Teachers = a.Teachers.ToCommaSpaceSeperatedString();
            Locations = a.Locations.ToCommaSpaceSeperatedString();
            Groups = a.Groups.ToCommaSpaceSeperatedString();
            Start = a.Start.Value;
            StartTimeSlot = a.StartTimeSlot;
            End = a.End.Value;
            Remark = a.Remark;
            ChangeDescription = a.ChangeDescription;

            if (!a.Valid.Value)
            {
                if (a.Moved.Value)
                    Status = AppointmentStatus.MovedInvalid;
                else
                    Status = AppointmentStatus.Invalid;
            }
            else
            {
                if (a.Cancelled.Value)
                    Status = AppointmentStatus.Cancelled;
                else if (a.Moved.Value)
                    Status = AppointmentStatus.MovedValid;
                else if (a.New.Value)
                    Status = AppointmentStatus.New;
                else
                    Status = AppointmentStatus.Normal;
            }
        }

        public string Subjects { get; set; }
        public string Teachers { get; set; }
        public string Locations { get; set; }
        public string Groups { get; set; }
        public DateTimeOffset Start { get; set; }
        public int? StartTimeSlot { get; set; }
        public DateTimeOffset End { get; set; }
        public string Remark { get; set; }
        public string ChangeDescription { get; set; }
        public AppointmentStatus Status { get; set; }

        public override int GetHashCode()
            => Subjects?.GetHashCode() ?? 0 ^
               Teachers?.GetHashCode() ?? 0 ^
               Locations?.GetHashCode() ?? 0 ^
               Groups?.GetHashCode() ?? 0 ^
               Start.GetHashCode() ^
               StartTimeSlot.GetHashCode() ^
               End.GetHashCode() ^
               Remark?.GetHashCode() ?? 0 ^
               ChangeDescription?.GetHashCode() ?? 0 ^
               Status.GetHashCode();

        public bool Equals(Appointment other)
            => (
                Subjects == other.Subjects &&
                Teachers == other.Teachers &&
                Locations == other.Locations &&
                Groups == other.Groups &&
                Start == other.Start &&
                StartTimeSlot == other.StartTimeSlot &&
                End == other.End &&
                Remark == other.Remark &&
                ChangeDescription == other.ChangeDescription &&
                Status == other.Status
            );
    }
}
