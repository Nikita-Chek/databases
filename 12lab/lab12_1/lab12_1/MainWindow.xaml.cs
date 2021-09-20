using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace lab12_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public class Candidate
        {
            public int Number { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public float Rating { get; set; }
            
            [Column(TypeName="Date")]
            public DateTime Birthday { get; set; }
            public ICollection<Confident> Confidents { get; set; }
            public ICollection<Promise> Promises { get; set; }
            public CandidateProfile Profile { get; set; }
            public Candidate()
            {
                Confidents = new List<Confident>();
                Promises = new List<Promise>();
                Birthday = DateTime.Now;
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
            // [Key] [ForeignKey("Candidate")] 
            public int Id { get; set; }
            public int Age { get; set; }
            public string Description { get; set; }
            public virtual Candidate Candidate { get; set; }
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
                modelBuilder.Entity<Candidate>().HasKey(p => p.Number);
                
                modelBuilder.Entity<Candidate>()
                    .HasMany(c => c.Confidents)
                    .WithOptional(c => c.Candidate)
                    .HasForeignKey(c => c.CandidateId).WillCascadeOnDelete(true);
                
                modelBuilder.Entity<Candidate>()
                    .HasMany(c => c.Promises)
                    .WithMany(c => c.Candidates);
                
                modelBuilder.Entity<Candidate>()
                    .HasRequired(c => c.Profile)
                    .WithRequiredPrincipal(c => c.Candidate)
                    .WillCascadeOnDelete(true);
                
                modelBuilder.Entity<CandidateProfile>().HasKey(p => p.Id);
                
                modelBuilder.Entity<Candidate>()
                    .Property(e => e.Birthday)
                    .HasColumnType("Date");
                // modelBuilder.Entity<Candidate>().Property(p=>p.Birthday).Has

            }

            public ElectionContext() : base("DBConnection")
            {
            }

            public DbSet<Candidate> Candidates { get; set; }
            public DbSet<CandidateProfile> CandidateProfiles { get; set; }
            public DbSet<Promise> Promises { get; set; }
            public DbSet<Confident> Confidents { get; set; }
        }


        ElectionContext context;


        public MainWindow()
        {
            using (ElectionContext db = new ElectionContext())
            {
                // Candidate c1 = new Candidate {Name = "Donald", Surname = "Trump", Rating = 25};
                // Candidate c2 = new Candidate {Name = "Barak", Surname = "Obama", Rating = 45};
                // Candidate c3 = new Candidate {Name = "John", Surname = "Travolta", Rating = 30};
                //
                // Confident conf1 = new Confident
                //     {FullName = "Kolya", Age = 19, PoliticalPreferences = "Neutral", Candidate = c1};
                // Confident conf2 = new Confident
                //     {FullName = "Katya", Age = 23, PoliticalPreferences = "Monarch", Candidate = c2};
                // Confident conf3 = new Confident
                //     {FullName = "Rita", Age = 21, PoliticalPreferences = "Liberal", Candidate = c2};
                //
                // CandidateProfile prof1 = new CandidateProfile {Id = c1.Number, Age = 50, Description = "Current president"};
                // CandidateProfile prof2 = new CandidateProfile {Id = c2.Number, Age = 40, Description = "Legend"};
                // CandidateProfile prof3 = new CandidateProfile {Id = c3.Number, Age = 30, Description = "Legend"};
                // c1.Profile = prof1;
                // c2.Profile = prof2;
                // c3.Profile = prof3;
                // Promise prom1 = new Promise {Text = "Stop War"};
                // Promise prom2 = new Promise {Text = "Increase revenue"};
                // prom1.Candidates.Add(c1);
                // prom2.Candidates.Add(c1);
                // prom2.Candidates.Add(c2);
                // db.Candidates.AddRange(new List<Candidate>() { c1, c2, c3 });
                // db.Confidents.AddRange(new List<Confident>() { conf1, conf2, conf3 });
                // db.Promises.AddRange(new List<Promise> { prom1, prom2 });
                // db.CandidateProfiles.AddRange(new List<CandidateProfile> { prof1, prof2 });
                // db.SaveChanges();
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
                MessageBox.Show(exception.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            context.Dispose();
        }
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
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

                            // context.Entry(curCand).Collection("Confidents").Load();
                            // foreach (var conf in curCand.Confidents)
                            //     context.Confidents.Remove(conf);
                            // var curProf = context.CandidateProfiles.Where(p => p.Candidate.Number == curCand.Number).Select(p=>p).FirstOrDefault();
                            // if (curProf != null)
                            //     context.CandidateProfiles.Remove(curProf);
                            
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
    }
}