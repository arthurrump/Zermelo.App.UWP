using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Zermelo.App.UWP.Controls
{
    public sealed partial class HamburgerButton : UserControl
    {
        public HamburgerButton()
        {
            this.InitializeComponent();
        }

        public Symbol Symbol
        {
            get { return (Symbol)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); SymbolIcon.Symbol = value; }
        }

        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register(nameof(Symbol), typeof(Symbol), typeof(HamburgerButton), new PropertyMetadata(Symbol.Home));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); TextBlock.Text = value; }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(HamburgerButton), new PropertyMetadata(string.Empty));
    }
}
