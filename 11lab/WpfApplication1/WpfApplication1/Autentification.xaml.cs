using System.Data.Entity;
using System.Windows;

namespace WpfApplication1
{
    public partial class Autentification : Window
    {
        private int type = 0;
        public Autentification()
        {
            InitializeComponent();
        }

        public new string Name
        {
            get { return NameBox.Text; }
        }
        
        public string Password
        {
            get { return PasswordBox.Text.GetHashCode().ToString(); }
        }

        public int Type
        {
            get { return type; }
        }
        private void login(object sender, RoutedEventArgs e)
        {
            type = 1;
            this.DialogResult = true;
        }

        private void registration(object sender, RoutedEventArgs e)
        {
            type = 2;
            this.DialogResult = true;
        }
    }
}