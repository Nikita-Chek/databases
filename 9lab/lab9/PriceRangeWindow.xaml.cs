using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace lab9
{
    public partial class PriceRangeWindow : Window
    {
        public PriceRangeWindow()
        {
            InitializeComponent();
        }
        private void SetPriceRange(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public float Min
        {
            get { return float.Parse(minStr.Text); }
        }

        public float Max
        {
            get { return float.Parse(maxStr.Text); }
        }

    }
}