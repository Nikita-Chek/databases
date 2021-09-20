using System;
using System.Data;
using System.Windows;

namespace lab10
{
    public partial class InsertWindow : Window
    {
        public InsertWindow()
        {
            InitializeComponent();
        }
        private void InsertButton(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text;
            string country = countryBox.Text;
            string phone = phoneBox.Text;
            string adress = adressBox.Text;

            if (name != "" && country != "" && phone != "" && adress != "")
            {
                try
                {
                    DataRow r = MainWindow.Global.shopDS.Tables["manufactory"].NewRow();
                    r["name"] = name;
                    r["country"] = country;
                    r["phone"] = Int32.Parse(phone);
                    r["adress"] = adress;
                    MainWindow.Global.shopDS.Tables["manufactory"].Rows.Add(r);
                    int ind = MainWindow.Global.shopDS.Tables["manufactory"].Rows.IndexOf(r);
                    String[] row = 
                    {
                        ind.ToString(), "","","","","Added"
                    };
                    // allDataGrid.ItemsSource = Global.shopDS.Tables["manufactory"].DefaultView;
                    MainWindow.Global.myStack.Push(row);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
            else
            {
                MessageBox.Show("input all rows!!!");
            }
        }
    }
}