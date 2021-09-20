using System;
using System.Data;
using System.Windows;

namespace lab9
{
    public partial class InsertProduct : Window
    {
        public InsertProduct()
        {
            InitializeComponent();
        }

        private void InsertButton(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text;
            string type = typeBox.Text;
            string manuf = manufBox.Text;
            string quantyty = quantytyBox.Text;
            string price = priceBox.Text;
            
            if (name != "" && type != "" && manuf != "" && quantyty != "" && price != "")
            {
                // DataRow dr = MainWindow.Global.shopDS.Tables["product"].NewRow();
                // dr["type"] = type;
                // dr["name"] = name;
                // dr["manuf_id"] = manuf;
                // dr["quantyty"] = quantyty;
                // dr["price"] = price;
                // MainWindow.Global.shopDS.Tables["product"].Rows.Add(dr);
            }
            else
            {
                MessageBox.Show("input all rows!!!");
                return;
            }
        }
    }
}