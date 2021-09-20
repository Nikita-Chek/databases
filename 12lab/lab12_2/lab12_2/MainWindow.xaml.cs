using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;


namespace lab12_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        ShopContext context;
        public static Stack<Tuple> myStack = new Stack<Tuple>();
        public static DataSet Worker = new DataSet("Worker");
        
        private string connectionString = $"server=localhost;" +
                                          $"user id=root;" +
                                          $"password=Nikitos2001p;" +
                                          $"database=lab9_add;";
        private MySqlDataAdapter workerAdapter;
        private MySqlCommandBuilder workerCommands;
        
        public MainWindow()
        {
            InitializeComponent();
            workerAdapter = new MySqlDataAdapter("select * from worker", connectionString);
            workerCommands = new MySqlCommandBuilder(workerAdapter);
            workerAdapter.Fill(Worker, "worker");
            Worker.Tables["worker"].PrimaryKey = new DataColumn[] { Worker.Tables["worker"].Columns["id"] };
            Worker.Tables["worker"].Columns["id"].AutoIncrement = true;
            var lastId = (from m in Worker.Tables["worker"].AsEnumerable()
                select m["id"]).Max();
            Worker.Tables["worker"].Columns["id"].AutoIncrementStep = 1;
            Worker.Tables["worker"].Columns["id"].AutoIncrementSeed = 1 + (int)lastId;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                context = new ShopContext();
                context.Manufactories.Load();
                mainGrid.ItemsSource = context.Manufactories.Local.ToBindingList();
                this.Closing += MainWindow_OnClosed;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            try
            {
                selectGrid.ItemsSource = Worker.Tables["worker"].DefaultView;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            context.Dispose();
        }
        
        private void edit_row_worker(object sender, DataGridCellEditEndingEventArgs e)
        {
            
        }
        private void delete_Row_worker(object sender, RoutedEventArgs e)
        {
            var i = selectGrid.SelectedIndex;
            (selectGrid.SelectedItems[0] as DataRowView).Row.Delete();
            Worker.Tables["worker"].Rows[i].Delete();
            workerAdapter.Update(Worker.Tables["worker"]);
        }

        private void insert_Row_worker(object sender, RoutedEventArgs e)
        {
            InsertWorker insert = new InsertWorker();
            if (insert.ShowDialog() == true)
            {
                try
                {
                    DataRow r = Worker.Tables["worker"].NewRow();
                    r["name"] = insert.Name;
                    r["manuf_id"] = insert.Manuf;
                    // MessageBox.Show("f");
                    Worker.Tables["worker"].Rows.Add(r);
                    workerAdapter.Update(Worker.Tables["worker"]);
                }
                catch(Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }
        private void edit_row(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Manufactory m = mainGrid.SelectedItems[0] as Manufactory;
                Manufactory n = new Manufactory(m);
                Tuple t = new Tuple(n, 1);
                myStack.Push(t);
                string edit = ((TextBox) e.EditingElement).Text;
                int col1 = e.Column.DisplayIndex - 0;
                try
                {
                    switch (col1)
                    {
                        case 0:
                            MessageBox.Show("Cant change Id");
                            break;
                        case 1:
                            m.name = edit;
                            break;
                        case 2:
                            m.coutry = edit;
                            break;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }

                context.SaveChanges();
            }
        }
        private void delete_Row(object sender, RoutedEventArgs e)
        {
            try 
            { 
                if(mainGrid.SelectedItems!=null) 
                { 
                    for(int i = 0; i < mainGrid.SelectedItems.Count; i++) 
                    { 
                        Manufactory m = mainGrid.SelectedItems[i] as Manufactory; 
                        if (m != null) 
                        {
                            myStack.Push(new Tuple(m, 2));
                            context.Manufactories.Remove(m);
                            
                            int id = Int16.Parse(m.id.ToString());
                            (from p in Worker.Tables["worker"].AsEnumerable()
                                where p.Field<int>("manuf_id") == id
                                select p).ToList<DataRow>().ForEach(row => row.Delete());
                            workerAdapter.Update(Worker.Tables["worker"]);
                        }
                    }
                } 
                context.SaveChanges();
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.ToString());
            }
        }
        private void insert_Row(object sender, RoutedEventArgs e)
        {
            Insert insert = new Insert();
            if (insert.ShowDialog() == true)
            {
                try
                {
                    Manufactory m = new Manufactory
                        {name = insert.Name, coutry = insert.Country};
                    context.Manufactories.Add(m);
                    myStack.Push(new Tuple(m, 3));
                    context.SaveChanges();
                }
                catch(Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }
        
        private void rollback(object sender, RoutedEventArgs e)
        {
            try
            {
                Tuple tup = myStack.Pop();
                switch (tup.item2)
                {
                    case 1:
                        Manufactory m = context.Manufactories.Where(p => p.id == tup.item1.id).Select(p=>p).FirstOrDefault();
                        m.coutry = tup.item1.coutry;
                        m.name = tup.item1.name; 
                        break;
                    case 2:
                        context.Manufactories.Add(tup.item1);
                        break;
                    case 3:
                        context.Manufactories.Remove(tup.item1);
                        break;
                }
                context.SaveChanges();
                MainWindow_OnLoaded(null,null);
                // context.Manufactories.Load();
                // mainGrid.ItemsSource = context.Manufactories.Local.ToBindingList();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private void select(object sender, RoutedEventArgs e)
        {
            // context.Clients.Load();
            // selectGrid.ItemsSource = context.Clients.Local.ToBindingList();
            try
            {
                context.Products.Load();
                selectGrid.ItemsSource = context.Products.Local.ToBindingList();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
        private void transaction(object sender, RoutedEventArgs e)
        {
            using (ShopContext context = new ShopContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Transaction insert = new Transaction();
                        if (insert.ShowDialog() == true)
                        {
                            Product p1 = context.Products.Where(p => p.id == insert.Id1).Select(p => p)
                                .FirstOrDefault();
                            Product p2 = context.Products.Where(p => p.id == insert.Id2).Select(p => p)
                                .FirstOrDefault();
                            int delta = 3;
                            p1.quantity = p1.quantity + delta;
                            if (context.ChangeTracker.HasChanges() == true)
                            {
                                p2.quantity = p2.quantity - delta;
                                if (p2.quantity < 0)
                                {
                                    transaction.Rollback();
                                }
                                else
                                {
                                    context.SaveChanges();
                                    context.Products.Load();
                                    selectGrid.ItemsSource = context.Products.Local.ToBindingList();
                                    transaction.Commit();
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void selectLINQ(object sender, RoutedEventArgs e)
        {using (ShopContext db = new ShopContext())
            { 
                // var result1 = db.Products.Where(x => x.quantity > 300).ToList();
                // selectGrid.ItemsSource = result1;
                
                // var result2 = db.Manufactories.Join(db.Products, 
                //     p => p.id, 
                //     c => c.manufId,
                //     (p, c) => new
                //     {
                //         Name = c.type,
                //         Type = p.name
                //     }).ToList();
                // selectGrid.ItemsSource = result2;
                //
                var result3 = db.Products.GroupBy(c => c.manufId)
                    .Select(g => 
                        new { ID = g.FirstOrDefault().id, AvgQua = g.Average(c => c.quantity) })
                    .Join(db.Products, 
                        a => a.ID, 
                        b => b.manufId, 
                        (a, b) => new 
                        {
                            Name = b.manufactory.name, 
                            Prod = b.name, 
                            Qua = a.AvgQua
                        }).ToList(); 
                selectGrid.ItemsSource = result3;
                
                // var result4 = db.Manufactories.Where(x => x.id = 1).SelectMany(x =>
                //     var result4 = db.Manufactories.SelectMany(x =>
                //     x.Products.Select(u=> new
                //     {
                //         NAME = x.name,
                //         PROD= u.name
                //     })).ToList();
                // selectGrid.ItemsSource = result4;
            }
        }

        private void procedure(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlParameter par = new MySqlParameter("@name", "Zenit");
                MySqlParameter par1 = new MySqlParameter("@out",null);
                context.Database.ExecuteSqlCommand("call count_product(\"Zenit\", nul");
                MessageBox.Show("d");
                MessageBox.Show(par1.Value.ToString());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}