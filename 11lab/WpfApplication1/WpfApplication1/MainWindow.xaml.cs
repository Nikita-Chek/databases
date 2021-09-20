using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    public partial class MainWindow
    { 
        public class Candidate
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public float Rating { get; set; }
            public ICollection<Confident> Confidents { get; set; }
            public ICollection<Promise> Promises { get; set; }
            public Candidate()
            {
                Confidents = new List<Confident>();
                Promises = new List<Promise>();
            }
        }
        public class Confident
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string PoliticalPreferences { get; set; }
            public int Age { get; set; }
            public int? CandidateId { get; set; }
            public Candidate Candidate { get; set; }
        }
        public class Promise
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public ICollection<Candidate>? Candidates { get; set; }
            public Promise()
            {
                Candidates = new List<Candidate>();
            }
        }
        public class CandidateProfile
        {
            [Key]
            [ForeignKey("Candidate")]
            public int Id { get; set; }
            public int Age { get; set; }
            public string Description { get; set; }
            public virtual Candidate Candidate { get; set; }
        }
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
        }
        class ElectionContext : DbContext
        {
            // static ElectionContext()
            // {
            //     System.Data.Entity.Database.SetInitializer<ElectionContext>(new MyContexInitializer);
            // }
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }

            public ElectionContext() : base("DBConnection")
            {
            }
            public DbSet<Candidate> Candidates { get; set; }
            public DbSet<CandidateProfile> CandidateProfiles { get; set; }
            public DbSet<Promise> Promises { get; set; }
            public DbSet<Confident> Confidents { get; set; }
            public DbSet<User> Users { get; set; }
        }
        
        
        ElectionContext context;
        
        
        public MainWindow()
        {
            using (ElectionContext db = new ElectionContext())
            {
                Candidate c1 = new Candidate { Name = "Donald", Surname = "Trump", Rating = 25 };
                Candidate c2 = new Candidate { Name = "Barak", Surname = "Obama", Rating = 45 };
                Candidate c3 = new Candidate { Name = "John", Surname = "Travolta", Rating = 30 };
                
                Confident conf1 = new Confident { FullName = "Kolya", Age = 19, PoliticalPreferences = "Neutral", Candidate = c1 };
                Confident conf2 = new Confident { FullName = "Katya", Age = 23, PoliticalPreferences = "Monarch", Candidate = c2 };
                Confident conf3 = new Confident { FullName = "Rita", Age = 21, PoliticalPreferences = "Liberal", Candidate = c2 };
                
                CandidateProfile prof1 = new CandidateProfile { Id = c1.Id, Age = 50, Description = "Current president" };
                CandidateProfile prof2 = new CandidateProfile { Id = c2.Id, Age = 40, Description = "Legend" };
                
                Promise prom1 = new Promise { Text = "Stop War" };
                Promise prom2 = new Promise { Text = "Increase revenue" };
                prom1.Candidates.Add(c1);
                prom2.Candidates.Add(c1);
                prom2.Candidates.Add(c2);
                // User user = new User
                // {
                //     Name = "Mike",
                //     Password = "qwerty".GetHashCode().ToString()
                // };
                // db.Users.Add(user);
                // db.SaveChanges();
                
                Candidate[] candList = new Candidate[] {c1, c2, c3};
                Confident[] confList = new Confident[] {conf1, conf2, conf3};
                CandidateProfile[] candprofList = new[] {prof1, prof2};
                Promise[] promList = new[] {prom1, prom2};

                Candidate cand = null;
                foreach (var x in candList)
                {
                    cand = db.Candidates.Where(c => c.Name == x.Name && c.Surname == x.Surname).Select(c => c).FirstOrDefault();
                    if (cand == null)
                        db.Candidates.Add(x);
                }
                db.SaveChanges();
                
                Confident conf = null;
                foreach (var x in confList)
                {
                    conf = db.Confidents.Where(c => c.FullName == x.FullName && c.Age == x.Age).Select(c => c).FirstOrDefault();
                    if (conf == null)
                    {
                        db.Confidents.Add(x);
                        db.SaveChanges();
                    }
                }

                CandidateProfile candprof = null;
                foreach (var x in candprofList)
                {
                    candprof = db.CandidateProfiles.Where(c => c.Id == x.Id).Select(c => c).FirstOrDefault();
                    if (cand == null)
                        db.CandidateProfiles.Add(x);
                }
                db.SaveChanges(); 

                Promise prom = null;
                foreach (var x in promList)
                {
                    prom = db.Promises.Where(c => c.Text == x.Text).Select(p => p).FirstOrDefault();
                    if (prom == null)
                        db.Promises.Add(x);
                }
                db.SaveChanges();

                // db.Candidates.AddRange(new List<Candidate>() { c1, c2, c3 });
                // db.Confidents.AddRange(new List<Confident>() { conf1, conf2, conf3 });
                // db.Promises.AddRange(new List<Promise> { prom1, prom2 });
                // db.CandidateProfiles.AddRange(new List<CandidateProfile> { prof1, prof2 });
                try
                {
                    
                    bool flag = true;
                    while (flag)
                    { 
                        Autentification auf = new Autentification(); 
                        if (auf.ShowDialog() == true) 
                        {
                            switch (auf.Type)
                            {
                                case 0:
                                    MessageBox.Show("Choose");
                                    break;
                                case 1:
                                    User loguser = db.Users.Where(p => p.Name == auf.Name).Select(p => p)
                                        .FirstOrDefault();
                                    if (loguser != null)
                                    {
                                        if (loguser.Password.Equals(auf.Password))
                                        {
                                            flag = false;
                                        }
                                        else
                                        {
                                            MessageBox.Show("wrong pass");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No such user");
                                    }

                                    break;
                                case 2:
                                    User user = new User
                                    {
                                        Name = auf.Name,
                                        Password = auf.Password
                                    };
                                    db.Users.Add(user);
                                    db.SaveChanges();
                                    flag = false;
                                    break;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    throw;
                }
            }
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                context = new ElectionContext();
                context.Candidates.Load();
                mainGrid.ItemsSource = context.Candidates.Local.ToBindingList();
                this.Closing += Window_Closed;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            context.Dispose();
        }
        
        private void delete_Row(object sender, RoutedEventArgs e)
        {
            try
            {
                if(mainGrid.SelectedItems!=null)
                {
                    for(int i = 0; i < mainGrid.SelectedItems.Count; i++)
                    {
                        Candidate curCand = mainGrid.SelectedItems[i] as Candidate;
                        if (curCand != null)
                        {
                            // var curConf = context.Confidents.Where(c => c.CandidateId == curCand.Id).Select(p => p).ToList();
                            // foreach (var x in curConf)
                            //     context.Confidents.Remove(x);

                            context.Entry(curCand).Collection("Confidents").Load();
                            foreach (var conf in curCand.Confidents)
                                context.Confidents.Remove(conf);
                            var curProf = context.CandidateProfiles.Where(p => p.Candidate.Id == curCand.Id).Select(p=>p).FirstOrDefault();
                            if (curProf != null)
                                context.CandidateProfiles.Remove(curProf);
                            
                            context.Candidates.Remove(curCand);
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
            InsertWindow insert = new InsertWindow();
            if (insert.ShowDialog() == true)
            {
                try
                {
                    Candidate c1 = new Candidate
                        {Name = insert.Name, Surname = insert.Surname, Rating = Int32.Parse(insert.Rating)};
                    context.Candidates.Add(c1);
                    context.SaveChanges();
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
                Candidate curCand = mainGrid.SelectedItems[0] as Candidate;
                string edit = ((TextBox) e.EditingElement).Text;
                int col1 = e.Column.DisplayIndex-0;
                // int row1 = e.Row.GetIndex();
                // string colomnName = mainGrid.SelectedCells[col1].Column.Header.ToString();
                // TextBlock t = mainGrid.Columns[0].GetCellContent(mainGrid.Items[row1]) as TextBlock;
                try
                {
                    switch (col1)
                    {
                        case 0:
                            MessageBox.Show("Cant change Id");
                            break;
                        case 1:
                            curCand.Name = edit;
                            break;
                        case 2:
                            curCand.Surname = edit;
                            break;
                        case 3:
                            curCand.Rating = Int32.Parse(edit);
                            break;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                context.SaveChanges();
                // dataGrid1.Columns[0].GetCellContent(dataGrid1.Items[2])
            }
        }
        private void select(object sender, RoutedEventArgs e)
        {
            context.Promises.Load();
            selectGrid.ItemsSource = context.Promises.Local.ToBindingList();
        }
        private void selct_LINQ(object sender, RoutedEventArgs e)
        {
            using (ElectionContext db = new ElectionContext())
            { 
                // var result1 = db.Candidates.Where(x => x.Rating > 10).ToList();
                //     selectGrid.ItemsSource = result1;
                //
                // var result2 = db.Candidates.Join(db.Confidents, 
                //     p => p.Id, 
                //     c => c.CandidateId,
                //     (p, c) => new
                //     {
                //         Name = c.FullName,
                //         CandidateSurname = p.Surname
                //     }).ToList();
                // selectGrid.ItemsSource = result2;
                //
                // var result3 = db.Confidents.GroupBy(c => c.CandidateId)
                //     .Select(g => 
                //         new { ID = g.FirstOrDefault().Id, AvgAge = g.Average(c => c.Age) })
                //     .Join(db.Candidates, 
                //         a => a.ID, 
                //         b => b.Id, 
                //         (a, b) => new 
                //         {
                //             Name = b.Name, 
                //             Surname = b.Surname, 
                //             Age = a.AvgAge
                //         }).ToList(); 
                // selectGrid.ItemsSource = result3;
                
                // var result4 = db.Candidates.Where(x => x.Name == "Donald").SelectMany(x =>
                var result4 = db.Candidates.SelectMany(x =>
                    x.Promises.Select(u=> new
                    {
                        NAME = x.Name,
                        PROMISE = u.Text
                    })).ToList();
                selectGrid.ItemsSource = result4;
            }
        }

        private void insert_Promes(object sender, RoutedEventArgs e)
        {
            InsertWindowPromeses insert = new InsertWindowPromeses();
            if (insert.ShowDialog() == true)
            {
                string[] idstr = insert.Ids.Split(' ');
                try
                {
                    int j;
                    var candidates = new List<Candidate>();
                    foreach (var i in idstr)
                    {
                        j = Int32.Parse(i);
                        candidates.Add(context.Candidates.First(x => x.Id == j));
                    }
                    Promise prom = new Promise { Text = insert.Promes };
                    foreach (var x in candidates)
                    {
                        prom.Candidates.Add(x);
                    }
                    context.Promises.Add(prom);
                    context.SaveChanges();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }
    }
}