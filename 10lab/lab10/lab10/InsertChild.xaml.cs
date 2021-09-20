using System;
using System.Data;
using System.Windows;

namespace lab10
{
    public partial class InsertChild : Window
    {
        public InsertChild()
        {
            InitializeComponent();
        }
        private void InsertButton(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text;
            string type = typeBox.Text;
            string quantyty = quantytyBox.Text;
            string price = priceBox.Text;

            if (name != "" && type != "" && quantyty != "" && price != "")
            {
                try
                {
                    DataRow r = MainWindow.Global.shopDS.Tables["product"].NewRow();
                    r["name"] = name;
                    r["type"] = type;
                    r["manuf_id"] = MainWindow.Global.index;
                    r["quantyty"] = quantyty;
                    r["price"] = price;
                    MainWindow.Global.shopDS.Tables["product"].Rows.Add(r);
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