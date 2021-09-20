using System;
using System.Windows;

namespace lab12_2
{
    public partial class InsertWorker : Window
    {
        public InsertWorker()
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

        public int Manuf
        {
            get { return Int16.Parse(manufBox.Text); }
        }
    }
}