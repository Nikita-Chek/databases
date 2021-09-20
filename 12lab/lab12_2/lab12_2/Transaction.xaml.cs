using System;
using System.Windows;

namespace lab12_2
{
    public partial class Transaction : Window
    {
        public Transaction()
        {
            InitializeComponent();
        }

        private void InsertButton(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public int Id1
        {
            get { return Int16.Parse(id1Box.Text); }
        }

        public int Id2
        {
            get { return Int16.Parse(id2Box.Text); }
        }
    }
}