using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Shell
{
    public sealed partial class TextHamburgerButton : UserControl
    {
        public TextHamburgerButton()
        {
            this.InitializeComponent();
        }

        public string SymbolText
        {
            get { return (string)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }

        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register(nameof(SymbolText), typeof(string), typeof(TextHamburgerButton), new PropertyMetadata(string.Empty));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextHamburgerButton), new PropertyMetadata(string.Empty));
    }
}
