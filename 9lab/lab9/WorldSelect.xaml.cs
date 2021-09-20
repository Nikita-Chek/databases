using System;
using System.Data;
using System.Linq;
using System.Windows;
using MySql.Data.MySqlClient;

namespace lab9
{
    public partial class WorldSelect : Window
    {
        private string connectionString = $"server=localhost;" +
                                          $"user id=root;" +
                                          $"password=Nikitos2001p;" +
                                          $"database=world;";
        private MySqlDataAdapter countryAdapter;
        private MySqlDataAdapter languageAdapter;
        private MySqlDataAdapter cityAdapter;
        private MySqlCommandBuilder countryCommands;
        private MySqlCommandBuilder languageCommands;
        private MySqlCommandBuilder cityCommands;
        DataSet world = new DataSet("world");
        
        public WorldSelect()
        {
            InitializeComponent();
            countryAdapter = new MySqlDataAdapter("select * from country", connectionString);
            languageAdapter = new MySqlDataAdapter("select * from countryLanguage", connectionString);
            cityAdapter = new MySqlDataAdapter("select * from city", connectionString);
            
            countryCommands = new MySqlCommandBuilder(countryAdapter);
            languageCommands = new MySqlCommandBuilder(languageAdapter);
            cityCommands = new MySqlCommandBuilder(cityAdapter);

            countryAdapter.Fill(world, "country");
            languageAdapter.Fill(world, "countryLanguage");
            cityAdapter.Fill(world, "city");
            
            DataRelation cityToCountry = new
                DataRelation("citytocountry", world.Tables["country"].Columns["code"],
                    world.Tables["city"].Columns["countrycode"]);
            DataRelation languageToCountry = new
                DataRelation("languagetocountry", world.Tables["country"].Columns["code"],
                    world.Tables["countrylanguage"].Columns["countrycode"]);

            world.Relations.Add(cityToCountry);
            world.Relations.Add(languageToCountry);
        }

        private void WorldSelect_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                worldGrid.ItemsSource = world.Tables["country"].DefaultView;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void select1(object sender, RoutedEventArgs e)
        {
            var quary = from c in world.Tables["country"].AsEnumerable()
                group c by c["continent"]
                into g
                select new
                {
                    name = g.Key, max = g.Max(x => x["GNP"])
                };
            var result = (from m in quary.AsEnumerable()
                join country in world.Tables["country"].AsEnumerable()
                    on m.max equals country["GNP"]
                select new
                {
                    Country = country["name"],
                    Continent = m.name,
                    Max = m.max
                }).Take(7);
            
            DataTable resultF = new DataTable();
            resultF.Columns.Add(new DataColumn("Country", System.Type.GetType("System.String")));
            resultF.Columns.Add(new DataColumn("Continent", System.Type.GetType("System.String")));
            resultF.Columns.Add(new DataColumn("Max", System.Type.GetType("System.Double")));
            foreach (var x in result)
            {
                DataRow newRow = resultF.Rows.Add();
                newRow.SetField("Country", x.Country);
                newRow.SetField("Continent", x.Continent);
                newRow.SetField("Max", x.Max);
            }
            worldGrid.ItemsSource = resultF.DefaultView;
        }

        private void select2(object sender, RoutedEventArgs e)
        {
            var query = from city in world.Tables["city"].AsEnumerable()
                join country in world.Tables["country"].AsEnumerable()
                    on city["countrycode"] equals country["code"]
                join countryl in world.Tables["countrylanguage"].AsEnumerable()
                    on country["code"] equals countryl["countrycode"]
                where (string) countryl["language"] == "Russian"
                select new
                {
                    City = city["name"],
                    Country = country["name"]
                };
                
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn("Country", System.Type.GetType("System.String")));
            result.Columns.Add(new DataColumn("City", System.Type.GetType("System.String")));
            foreach (var x in query)
            {
                DataRow newRow = result.Rows.Add();
                newRow.SetField("Country", x.Country);
                newRow.SetField("City", x.City);
            }
            worldGrid.ItemsSource = result.DefaultView;
        }

        private void select3(object sender, RoutedEventArgs e)
        {
            var query1 = (from country in world.Tables["country"].AsEnumerable()
                join city in world.Tables["city"].AsEnumerable()
                    on country["code"] equals city["countrycode"]
                where (string) country["code"] == "BLR"
                orderby city["population"] descending
                select new
                {
                    Cont = country["name"], City = city["name"]
                }).Take(5);
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn("Name", System.Type.GetType("System.String")));
            result.Columns.Add(new DataColumn("City", System.Type.GetType("System.String")));
            
            foreach (var x in query1)
            {
                DataRow newRow = result.Rows.Add();
                newRow.SetField("Name", x.Cont);
                newRow.SetField("City", x.City);
            }
            worldGrid.ItemsSource = result.DefaultView;
        }
    }
}