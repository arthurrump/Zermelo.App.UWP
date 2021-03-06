﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Zermelo.App.UWP.Helpers
{
    static class ObservableCollectionExtensions
    {
        public static void MorphInto<TSource>(this ObservableCollection<TSource> first, IEnumerable<TSource> second)
        {
            var add = second.Except(first).ToList();
            var remove = first.Except(second).ToList();


            foreach (var i in remove)
                first.Remove(i);

            foreach (var i in add)
                first.Add(i);

            // If there are any changes to first, make sure it's in 
            // the same order as second.
            if (add.Count() > 0 || remove.Count() > 0)
                for (int i = 0; i < second.Count(); i++)
                    first.Move(first.IndexOf(second.ElementAt(i)), i);
        }
    }
}
