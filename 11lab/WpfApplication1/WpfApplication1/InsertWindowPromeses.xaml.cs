using System.Collections.Generic;
using System.Windows;

namespace WpfApplication1
{
    public partial class InsertWindowPromeses : Window
    {
        public InsertWindowPromeses()
        {
            InitializeComponent();
        }
        private void InsertButton(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        
        public string Promes
        {
            get { return promBox.Text; }
        }

        public string Ids
        {
            get { return idBox.Text; }
        }
    }
}