
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace lab12_2.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<lab12_2.ShopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    } 
}