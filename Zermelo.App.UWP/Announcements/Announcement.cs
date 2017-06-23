using System;
using System.Collections.Generic;

namespace Zermelo.App.UWP.Announcements
{
    public class Announcement : IEquatable<Announcement>
    {
        public static List<string> Fields => new List<string> { "title", "text" };

        public Announcement() { } // Need this for JSON serialization

        public Announcement(API.Models.Announcement a)
        {
            Title = a.Title;
            Text = a.Text;
        }

        public string Title { get; set; }
        public string Text { get; set; }

        public override int GetHashCode() 
            => Title?.GetHashCode() ?? 0 ^ 
               Text ?.GetHashCode() ?? 0;

        public bool Equals(Announcement other)
            => (
                Title == other.Title &&
                Text == other.Text
            );
    }
}
