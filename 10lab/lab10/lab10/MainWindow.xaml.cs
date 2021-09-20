using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace lab10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
        
        public MainWindow()
        {
            InitializeComponent();
            manufactoryAdapter = new MySqlDataAdapter("select * from manufactory", connectionString);
            productAdapter = new MySqlDataAdapter("select * from product", connectionString);
            // customerAdapter = new MySqlDataAdapter("select * from customer", connectionString);
            // sellsAdapter = new MySqlDataAdapter("select * from sells", connectionString);


            manufactoryCommands = new MySqlCommandBuilder(manufactoryAdapter);
            productCommands = new MySqlCommandBuilder(productAdapter);
            // customerCommands = new MySqlCommandBuilder(customerAdapter);
            // sellsCommands = new MySqlCommandBuilder(sellsAdapter);

            
            manufactoryAdapter.Fill(Global.shopDS, "manufactory");
            productAdapter.Fill(Global.shopDS, "product");
            // customerAdapter.Fill(Global.shopDS, "customer");
            // sellsAdapter.Fill(Global.shopDS, "sells");
            //
            
            Global.shopDS.Tables["product"].PrimaryKey = new DataColumn[] { Global.shopDS.Tables["product"].Columns["id"] };
            Global.shopDS.Tables["manufactory"].PrimaryKey = new DataColumn[1] { Global.shopDS.Tables["manufactory"].Columns["id"] };
            
            DataRelation manufactoryToProduct = new
                DataRelation("ManufactoryProduct", Global.shopDS.Tables["manufactory"].Columns["id"],
                    Global.shopDS.Tables["product"].Columns["manuf_id"]);
            // DataRelation productToSell = new
            //     DataRelation("ProductSells", Global.shopDS.Tables["product"].Columns["id"],
            //         Global.shopDS.Tables["sells"].Columns["prod_id"]);
            // DataRelation customerToSell = new
            //     DataRelation("CustomerSells", Global.shopDS.Tables["customer"].Columns["id"],
            //         Global.shopDS.Tables["sells"].Columns["cust_id"]);
            
            Global.shopDS.Relations.Add(manufactoryToProduct);
            // Global.shopDS.Relations.Add(productToSell);
            // Global.shopDS.Relations.Add(customerToSell);
            //
            manufactoryToProduct.ChildKeyConstraint.DeleteRule = Rule.Cascade;
            
            Global.shopDS.Tables["product"].Columns["id"].AutoIncrement = true;
            var lastId = (from m in Global.shopDS.Tables["product"].AsEnumerable()
                select m["id"]).Max();
            Global.shopDS.Tables["product"].Columns["id"].AutoIncrementSeed = (int)lastId + 1;
            Global.shopDS.Tables["product"].Columns["id"].AutoIncrementStep = 1;
            
            Global.shopDS.Tables["manufactory"].Columns["id"].AutoIncrement = true;
            lastId = (from m in Global.shopDS.Tables["manufactory"].AsEnumerable()
                select m["id"]).Max();
            Global.shopDS.Tables["manufactory"].Columns["id"].AutoIncrementSeed = (int)lastId + 1;
            Global.shopDS.Tables["manufactory"].Columns["id"].AutoIncrementStep = 1;

        }
        public static class Global
        {
            public static string table = String.Empty;
            public static DataSet shopDS = new DataSet("Shop");
            public static float max = 0;
            public static float min = 0;
            public static Stack myStack = new Stack();
            public static string index;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectDataGrid.ItemsSource = Global.shopDS.Tables["manufactory"].DefaultView;
                Global.table = "manufactory";
                SelectDataGrid.ItemsSource = Global.shopDS.Tables["product"].DefaultView;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        private void showProperty(string colname)
        {
            string colproperty = "";
            colproperty += "Name - > " + Global.shopDS.Tables["manufactory"].Columns[colname].ColumnName + "\n";
            colproperty += "Type - > " + Global.shopDS.Tables["manufactory"].Columns[colname].DataType + "\n";
            colproperty += "Allow NULL - > " + Global.shopDS.Tables["manufactory"].Columns[colname].AllowDBNull + "\n";
            colproperty += "Autoincrement - > " + Global.shopDS.Tables["manufactory"].Columns[colname].AutoIncrement + "\n";
            colproperty += "Unique - > " + Global.shopDS.Tables["manufactory"].Columns[colname].Unique + "\n";
            colproperty += "Number of primary keys - >" + Global.shopDS.Tables["manufactory"].PrimaryKey.Length + "\n";
            MessageBox.Show(colproperty);
        }
        private void DataGrid_RowDelete(object sender, RoutedEventArgs e)
        {
            try
            {
                var r = (allDataGrid.SelectedItems[0] as DataRowView).Row.ItemArray;
                var i = allDataGrid.SelectedIndex;
                (allDataGrid.SelectedItems[0] as DataRowView).Row.Delete();
                Global.shopDS.Tables["manufactory"].Rows[i].Delete();
                Console.WriteLine(Global.shopDS.Tables["manufactory"].Rows[i].RowState.ToString());
                String[] row = 
                {
                    r.GetValue(0).ToString(), r.GetValue(1).ToString(),
                    r.GetValue(2).ToString(), r.GetValue(3).ToString(),
                    r.GetValue(4).ToString(), "Deleted"
                };
                // allDataGrid.ItemsSource = Global.shopDS.Tables["manufactory"].DefaultView;
                Global.myStack.Push(row);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                allDataGrid.SelectedItems.Clear();
            }
        }
        
        private void DataGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                
                string edit = ((TextBox) e.EditingElement).Text;
                int col1 = e.Column.DisplayIndex - 1;
                int row1 = e.Row.GetIndex();
                Console.WriteLine(edit);
                Global.shopDS.Tables["manufactory"].AcceptChanges();
                var r = Global.shopDS.Tables["manufactory"].Rows[row1].ItemArray;
                String[] row = 
                {
                    r.GetValue(0).ToString(), r.GetValue(1).ToString(),
                    r.GetValue(2).ToString(), r.GetValue(3).ToString(),
                    r.GetValue(4).ToString(), "Modified", row1.ToString()
                };
                Console.WriteLine(r.GetValue(4));
                Global.shopDS.Tables["manufactory"].Rows[row1][col1] = edit;
                allDataGrid.ItemsSource = Global.shopDS.Tables[Global.table].DefaultView;
                Global.myStack.Push(row);
            }
        }

        private void DataGrid_RowInsert(object sender, RoutedEventArgs e)
        {
            
            InsertWindow insert = new InsertWindow();
            insert.ShowDialog();
        }
        private void rollBack(object sender, RoutedEventArgs e)
        {
            // Global.shopDS.Tables["manufactory"].Rows.Add();
            if (Global.myStack.Count != 0)
            {
                String[] str = (String[])Global.myStack.Pop();
                switch (str[5])
                {
                    case "Deleted":
                        Console.WriteLine("Delete backroll");
                        try
                        {
                            Global.shopDS.Tables["manufactory"].Rows.Add(new Object[]
                            {
                                Int32.Parse(str[0]),
                                str[1],str[2],
                                Int32.Parse(str[3]),
                                str[4]
                            });
                            allDataGrid.ItemsSource = Global.shopDS.Tables["manufactory"].DefaultView;
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception.Message);
                        }
                        break;
                    case "Modified":
                        try
                        {
                            Console.WriteLine("Edit backroll");
                            Console.WriteLine(str[6]);
                            Global.shopDS.Tables["manufactory"].Rows[Int32.Parse(str[6])].Delete();
                            Global.shopDS.Tables["manufactory"].Rows.Add(new Object[]
                            {
                                Int32.Parse(str[0]),
                                str[1],str[2],
                                Int32.Parse(str[3]),
                                str[4]
                            });
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception.Message);
                        }
                        break;
                    case "Added":
                        Console.WriteLine("Add rollback");
                        Global.shopDS.Tables["manufactory"].Rows[Int32.Parse(str[0])].Delete();
                        break;
                }
            }
        }
        private void dataBaseMerge(object sender, RoutedEventArgs e)
        {
            int n = manufactoryAdapter.Update(Global.shopDS.Tables["manufactory"]);
            Console.WriteLine(n);
        }
        private void Button_OnChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton) sender;
            showProperty(radioButton.Content.ToString());
        }
        private void rowState(object sender, RoutedEventArgs routedEventArgs)
        {
            if (allDataGrid.SelectedItems.Count != 0)
            {
                try
                {
                    Console.WriteLine((allDataGrid.SelectedItems[0] as DataRowView)?.Row.RowState.ToString());
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void updateLINQ(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.shopDS.Tables["manufactory"].AcceptChanges();
                (from p in Global.shopDS.Tables["manufactory"].AsEnumerable()
                    where p.Field<string>("country") == updateBox.Text
                    select p).ToList<DataRow>().ForEach(row => { row["adress"] = "Mogilev"; });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                allDataGrid.SelectedItems.Clear();
            }
        }
        
        private void deleteLINQ(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.shopDS.Tables["manufactory"].AcceptChanges();
                (from p in Global.shopDS.Tables["manufactory"].AsEnumerable()
                    where p.Field<string>("country") == updateBox.Text
                    select p).ToList<DataRow>().ForEach(row => { row.Delete(); });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                allDataGrid.SelectedItems.Clear();
            }
        }

        private void insertChild(object sender, RoutedEventArgs e)
        {
            try
            {
                var r = (allDataGrid.SelectedItems[0] as DataRowView).Row.ItemArray;
                Global.index = r.GetValue(0).ToString();
                InsertChild insert = new InsertChild();
                insert.ShowDialog();
                SelectDataGrid.ItemsSource = Global.shopDS.Tables["product"].DefaultView;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}