using System.Windows;

namespace lab12_2
{
    public partial class Insert : Window
    {
        public Insert()
        {
            InitializeComponent();
        }
        private void InsertButton(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        
        public string Name
        {
            get { return nameBox.Text; }
        }

        public string Country
        {
            get { return countryBox.Text; }
        }

    }
}