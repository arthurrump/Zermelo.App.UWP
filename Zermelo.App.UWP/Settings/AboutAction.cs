using System;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Settings
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
