using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zermelo.App.UWP.Schedule;

namespace Zermelo.App.UWP.OtherSchedules
{
    public class SearchItem : IEquatable<SearchItem>
    {
        public SearchItem() { }

        public SearchItem(API.Models.User user, ScheduleType type)
        {
            Type = type;
            Code = user.Code;
            DisplayText = $"{user.FullName} ({user.Code})";
        }

        public SearchItem(API.Models.Group group)
        {
            Type = ScheduleType.Group;
            Code = group.Id.Value.ToString();
            DisplayText = group.ExtendedName;
        }

        public SearchItem(API.Models.Location location)
        {
            Type = ScheduleType.Location;
            Code = location.Id.Value.ToString();
            DisplayText = location.Name;
        }

        public ScheduleType Type { get; set; }
        public string Code { get; set; }
        public string DisplayText { get; set; }

        public override int GetHashCode()
            => Type.GetHashCode() ^
               Code.GetHashCode();

        public bool Equals(SearchItem other)
            => Type == other.Type &&
               Code == other.Code;
    }
}
