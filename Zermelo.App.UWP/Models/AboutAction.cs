using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Models
{
    public class AboutAction
    {
        public AboutAction(string text, Symbol symbol, Action action)
        {
            Text = text;
            Symbol = symbol;
            Action = action;
        }

        public string Text { get; }
        public Symbol Symbol { get; }
        public Action Action { get; }
    }
}
