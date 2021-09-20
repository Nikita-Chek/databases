using System.Data.Entity;
using System.Windows;
using MySql.Data.Entity;

namespace lab12_2
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ShopContext:DbContext
    { 
        public ShopContext() : base("ShopDB")
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufactory>().HasMany(p => p.Products).
                WithOptional(p=>p.manufactory).HasForeignKey(p=>p.manufId).WillCascadeOnDelete(true);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Client>().HasIndex(p => p.name).IsUnique().HasName("index1");
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufactory> Manufactories { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}