using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace lab9
{
    public partial class MainWindow : Window
    {
        private string connectionString = $"server=localhost;" +
                                          $"user id=root;" +
                                          $"password=Nikitos2001p;" +
                                          $"database=lab9;";
        private MySqlDataAdapter manufactoryAdapter;
        private MySqlDataAdapter productAdapter;
        private MySqlDataAdapter customerAdapter;
        private MySqlDataAdapter sellsAdapter;
        private MySqlCommandBuilder manufactoryCommands;
        private MySqlCommandBuilder productCommands;
        private MySqlCommandBuilder customerCommands;
        private MySqlCommandBuilder sellsCommands;
        // DataSet shopDS = new DataSet("Shop");
        
        public MainWindow()
        {
            InitializeComponent();
            manufactoryAdapter = new MySqlDataAdapter("select * from manufactory", connectionString);
            productAdapter = new MySqlDataAdapter("select * from product", connectionString);
            customerAdapter = new MySqlDataAdapter("select * from customer", connectionString);
            sellsAdapter = new MySqlDataAdapter("select * from sells", connectionString);


            manufactoryCommands = new MySqlCommandBuilder(manufactoryAdapter);
            productCommands = new MySqlCommandBuilder(productAdapter);
            customerCommands = new MySqlCommandBuilder(customerAdapter);
            sellsCommands = new MySqlCommandBuilder(sellsAdapter);

            
            manufactoryAdapter.Fill(Global.shopDS, "manufactory");
            productAdapter.Fill(Global.shopDS, "product");
            customerAdapter.Fill(Global.shopDS, "customer");
            sellsAdapter.Fill(Global.shopDS, "sells");
            
            DataRelation manufactoryToProduct = new
                DataRelation("ManufactoryProduct", Global.shopDS.Tables["manufactory"].Columns["id"],
                    Global.shopDS.Tables["product"].Columns["manuf_id"]);
            DataRelation productToSell = new
                DataRelation("ProductSells", Global.shopDS.Tables["product"].Columns["id"],
                    Global.shopDS.Tables["sells"].Columns["prod_id"]);
            DataRelation customerToSell = new
                DataRelation("CustomerSells", Global.shopDS.Tables["customer"].Columns["id"],
                    Global.shopDS.Tables["sells"].Columns["cust_id"]);
            
            Global.shopDS.Relations.Add(manufactoryToProduct);
            Global.shopDS.Relations.Add(productToSell);
            Global.shopDS.Relations.Add(customerToSell);
        }

        public static class Global
        {
            public static string table = String.Empty;
            public static DataSet shopDS = new DataSet("Shop");
            public static float max = 0;
            public static float min = 0;
        }

        public void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                allDataGrid.ItemsSource = Global.shopDS.Tables["manufactory"].DefaultView;
                Global.table = "manufactory";
                SelectDataGrid.ItemsSource = Global.shopDS.Tables["product"].DefaultView;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void SetTable(object sender, RoutedEventArgs e)
        {
            if ((bool) radioButton1.IsChecked)
            {
                allDataGrid.ItemsSource = Global.shopDS.Tables["manufactory"]?.DefaultView;
                Global.table = "manufactory";
            }
            else if ((bool) radioButton2.IsChecked)
            {
                allDataGrid.ItemsSource = Global.shopDS.Tables["product"]?.DefaultView;
                Global.table = "product";
            }
            else
            {
                allDataGrid.ItemsSource = null;
                Global.table = String.Empty;
            }
        }
        
        
        private void SetPriceRange(object sender, RoutedEventArgs e)
        {
            PriceRangeWindow prw = new PriceRangeWindow();
            if (prw.ShowDialog() == true)
            {
                Global.max = prw.Max;
                Global.min = prw.Min;
            }   
            // var result = Global.shopDS.Tables["product"].Select("price < 6 and price > 4");
            // SelectDataGrid.ItemsSource = result.CopyToDataTable().DefaultView;
            var result = from p in Global.shopDS.Tables["product"].AsEnumerable()
                where (float)p["price"] > Global.min && (float)p["price"] < Global.max
                select p;
            SelectDataGrid.ItemsSource = result.CopyToDataTable().DefaultView;
        }

        private void SortByType(object sender, RoutedEventArgs e)
        {
            if ( new []{"Jesus", "Budda", "Allah"}.Contains(sortType.Text))
            {
                var result = from p in Global.shopDS.Tables["product"].AsEnumerable()
                    where (string) p["type"] == sortType.Text
                    select p;
                SelectDataGrid.ItemsSource = result.CopyToDataTable().DefaultView;
            }
            else
            {
                MessageBox.Show("Input Type!!!");
            }
        }

        private void FindNameLike(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(nameLike.Text))
            {
                try
                {
                    // var result = shopDS.Tables["product"].Select($"name like \"%{nameLike}%\"");
                    // SelectDataGrid.ItemsSource = result.CopyToDataTable().DefaultView;
                    var result = from p in Global.shopDS.Tables["product"].AsEnumerable()
                        where p["name"].ToString().Contains(nameLike.Text)
                        select p;
                    SelectDataGrid.ItemsSource = result.CopyToDataTable().DefaultView;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    MessageBox.Show("Nothing Found");
                }
            }
            else
            {
                MessageBox.Show("Input Type!!!");
            }
        }

        private void Group(object sender, RoutedEventArgs e)
        {
            var query = from p in Global.shopDS.Tables["product"].AsEnumerable()
                join m in Global.shopDS.Tables["manufactory"].AsEnumerable()
                    on p["manuf_id"] equals m["id"]
                where (string) p["type"] == "Jesus"
                select new {Name = p["name"], Manufactory = m["name"]};

            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn("Name", System.Type.GetType("System.String")));
            result.Columns.Add(new DataColumn("Manufactory", System.Type.GetType("System.String")));
            
            foreach (var VARIABLE in query)
            {
                DataRow newRow = result.Rows.Add();
                newRow.SetField("Name", VARIABLE.Name);
                newRow.SetField("Manufactory", VARIABLE.Manufactory);
            }
            
            SelectDataGrid.ItemsSource = result.DefaultView;

            WorldSelect ws = new WorldSelect();
            ws.Show();
        }

        private void Group1(object sender, RoutedEventArgs e)
        {
            // "quontity of cells in product"
            // var query = from s in Global.shopDS.Tables["sells"].AsEnumerable()
            //     join p in Global.shopDS.Tables["product"].AsEnumerable()
            //         on s["prod_id"] equals p["id"]
            //     group new{s,p} by p.Field<string>("name") into g
            //     select new
            //     {
            //         ProdName = g.Key,
            //         ProdSells = g.Count()
            //     };
            // DataTable result = new DataTable();
            // result.Columns.Add(new DataColumn("Name", System.Type.GetType("System.String")));
            // result.Columns.Add(new DataColumn("Qua", System.Type.GetType("System.Int32")));
            //
            // foreach (var x in query)
            // {
            //     DataRow newRow = result.Rows.Add();
            //     newRow.SetField("Name", x.ProdName);
            //     newRow.SetField("Qua", x.ProdSells);
            // }
            // SelectDataGrid.ItemsSource = result.DefaultView;

            

            // "sels on manufactory"
            // var query = from s in Global.shopDS.Tables["sells"].AsEnumerable()
            //     join p in Global.shopDS.Tables["product"].AsEnumerable()
            //         on s["prod_id"] equals p["id"]
            //     join m in Global.shopDS.Tables["manufactory"].AsEnumerable()
            //         on p["manuf_id"] equals m["id"]
            //     select new
            //     {
            //         name = m.Field<string>("name"),
            //         cust = s.Field<int>("cust_id"),
            //     }
            //     into temp1
            //     group temp1 by temp1.name
            //     into temp2
            //     select new
            //     {
            //         Mname = temp2.Key,
            //         Mcount = temp2.Count()
            //     };
            // DataTable result = new DataTable();
            // result.Columns.Add(new DataColumn("ManufName", System.Type.GetType("System.String")));
            // result.Columns.Add(new DataColumn("ManufQua", System.Type.GetType("System.Int32")));
            // foreach (var x in query)
            // {
            //     DataRow newRow = result.Rows.Add();
            //     newRow.SetField("ManufName", x.Mname);
            //     newRow.SetField("ManufQua", x.Mcount);
            // }
            // SelectDataGrid.ItemsSource = result.DefaultView;

            
            
            // "sum of prices by fullname"
            var query1 = from s in Global.shopDS.Tables["sells"].AsEnumerable()
                join p in Global.shopDS.Tables["product"].AsEnumerable()
                    on s["prod_id"] equals p["id"]
                join c in Global.shopDS.Tables["customer"].AsEnumerable()
                    on s["cust_id"] equals c["id"]
                select new
                {
                    Name = c["fullname"], Price = p["price"]
                };
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn("Name", System.Type.GetType("System.String")));
            result.Columns.Add(new DataColumn("Price", System.Type.GetType("System.Double")));
            
            foreach (var x in query1)
            {
                DataRow newRow = result.Rows.Add();
                newRow.SetField("Name", x.Name);
                newRow.SetField("Price", x.Price);
            }
            
            var query2 = from n in result.AsEnumerable()
                group n by n.Field<string>("Name")
                into g
                select new
                {
                    Fullname = g.Key,
                    Finalyprice = g.Sum(row => row.Field<double>("Price"))
                };
            DataTable resultFinaly = new DataTable();
            resultFinaly.Columns.Add(new DataColumn("Name", System.Type.GetType("System.String")));
            resultFinaly.Columns.Add(new DataColumn("Price", System.Type.GetType("System.Double")));
            
            foreach (var x in query2)
            {
                DataRow newRow = resultFinaly.Rows.Add();
                newRow.SetField("Name", x.Fullname);
                newRow.SetField("Price", x.Finalyprice);
            }
            SelectDataGrid.ItemsSource = resultFinaly.DefaultView;
        }

        
        
        
        private void AllDataGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                
                string edit = ((TextBox) e.EditingElement).Text;
                int col1 = e.Column.DisplayIndex - 1;
                int row1 = e.Row.GetIndex();
                Global.shopDS.Tables[Global.table].Rows[row1][col1] = edit;
                allDataGrid.ItemsSource = Global.shopDS.Tables[Global.table].DefaultView;
                try
                {
                    if (Global.table == "product")
                        productAdapter.Update(Global.shopDS, "product");
                    else if (Global.table == "manufactory")
                    {
                        int n = manufactoryAdapter.Update(Global.shopDS, "manufactory");
                        Console.WriteLine(n);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
        

        private void DataGrid_RowDelete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Sure?", "delete", MessageBoxButton.YesNo);
            if(dialogResult == MessageBoxResult.Yes)
            {
                try
                {
                    Console.WriteLine("sdswwdsds");
                    Global.shopDS.AcceptChanges();
                    DataRowView row = (DataRowView) allDataGrid.SelectedItem;
                    Global.shopDS.Tables[Global.table].Rows[0].Delete();
                    //(allDataGrid.SelectedItems[0] as DataRowView).Row.Delete();
                    Console.WriteLine(row.Row.RowState.ToString());
                    // manufactoryAdapter.Update(Global.shopDS.Tables["manufactory"]);
                    switch (Global.table)
                    {
                        case "product":
                            //MessageBox.Show("efjlej");
                            int n = productAdapter.Update(Global.shopDS.Tables["product"]);
                            Console.WriteLine(n);
                            break;
                        case "manufactory":
                            int m = manufactoryAdapter.Update(Global.shopDS.Tables["manufactory"]);
                            Console.WriteLine(m);
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                Console.WriteLine("Ok!");
            }
        }

        
        
        
        
        private void InsertValue(object sender, RoutedEventArgs e)
        {
            manufactoryAdapter.Update(Global.shopDS.Tables[Global.table]);
            // switch (Global.table)
            // {
;            //     case "product":
            //         // InsertProduct IP = new InsertProduct();
            //         // IP.Show();
            //         try
            //         {
            //             productAdapter.Update(Global.shopDS.Tables["product"]);
            //         }
            //         catch (Exception exception)
            //         {
            //             Console.WriteLine(exception);
            //         }
            //         break;
            //     case "manufactory":
            //         
            //         break;
            // }
        }
    }
}