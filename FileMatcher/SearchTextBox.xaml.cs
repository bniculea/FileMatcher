using System.Windows;
using System.Windows.Controls;

namespace FileMatcher
{
    public partial class SearchTextBox
    {
        public Image ImageSource
        {
            get { return base.GetValue(SourceProperty) as Image;}
            set { base.SetValue(SourceProperty, value);}
        }

        public TextBox TextBox
        {
            get { return base.GetValue(TextBoxProperty) as TextBox;}
            set { base.SetValue(TextBoxProperty, value);}
        }

        public static readonly  DependencyProperty SourceProperty = DependencyProperty.Register("ImageSource", typeof(Image), typeof(SearchTextBox));
        public static readonly DependencyProperty TextBoxProperty = DependencyProperty.Register("TextBoxProperty",typeof(TextBox),typeof(SearchTextBox) );

        public SearchTextBox()
        {
            InitializeComponent();
        }
    }
}
