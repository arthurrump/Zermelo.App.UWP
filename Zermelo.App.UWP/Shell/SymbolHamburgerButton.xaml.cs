using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Shell
{
    public sealed partial class SymbolHamburgerButton : UserControl
    {
        public SymbolHamburgerButton()
        {
            this.InitializeComponent();
        }

        public Symbol Symbol
        {
            get { return (Symbol)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }

        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register(nameof(Symbol), typeof(Symbol), typeof(SymbolHamburgerButton), new PropertyMetadata(Symbol.Home));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(SymbolHamburgerButton), new PropertyMetadata(string.Empty));
    }
}
