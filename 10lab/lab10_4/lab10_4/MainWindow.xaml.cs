using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using MySql.Data.MySqlClient;

namespace lab10_4
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
        private MySqlDataAdapter testAdapter;
        private MySqlCommandBuilder testBuilder;
        
        public MainWindow()
        {
            InitializeComponent();
            testAdapter = new MySqlDataAdapter("select * from test", connectionString);
            testBuilder = new MySqlCommandBuilder(testAdapter);
            testAdapter.Fill(Global.testSet, "test");

            testAdapter.InsertCommand = new MySqlCommand("insertRow");
            testAdapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            testAdapter.InsertCommand.Parameters.Add(new MySqlParameter("x", MySqlDbType.VarChar, 20, "field1"));
            testAdapter.InsertCommand.Parameters.Add(new MySqlParameter("y", MySqlDbType.VarChar, 20, "field2"));
            MySqlParameter param = new MySqlParameter("id", MySqlDbType.Int32, 0, "id");
            param.Direction = ParameterDirection.Output;
            testAdapter.InsertCommand.Parameters.Add(param);
        }
        public static class Global
        {
            public static DataSet testSet = new DataSet("Shop");
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                allDataGrid.ItemsSource = Global.testSet.Tables["test"].DefaultView;
                testAdapter.Update(Global.testSet,"testtable");
                Global.testSet.AcceptChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            testAdapter.Update(Global.testSet.Tables["test"]);
        }
    }
}