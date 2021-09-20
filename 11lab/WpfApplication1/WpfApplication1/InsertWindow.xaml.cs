using System;
using System.Windows;

namespace WpfApplication1
{
    public partial class InsertWindow : Window
    {
        public InsertWindow()
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

        public string Surname
        {
            get { return surnameBox.Text; }
        }

        public string Rating
        {
            get { return ratingBox.Text; }
        }
        // private void InsertButton(object sender, RoutedEventArgs e)
        // {
        //     string name = nameBox.Text;
        //     string surname = surnameBox.Text;
        //     string rating = ratingBox.Text;
        //
        //     if (name != "" && surname != "" && rating != "")
        //     {
        //         try
        //         {
        //             MainWindow.Candidate c1 = new MainWindow.Candidate { Name = name, Surname = surname, Rating = Int32.Parse(rating) };
        //         }
        //         catch (Exception exception)
        //         {
        //             Console.WriteLine(exception);
        //         }
        //     }
        //     else
        //     {
        //         MessageBox.Show("input all rows!!!");
        //     }
        // }
    }
}