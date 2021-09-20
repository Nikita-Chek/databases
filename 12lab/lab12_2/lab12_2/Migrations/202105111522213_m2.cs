namespace lab12_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2 : DbMigration
    {
        public override void Up()
        {
            //CreateIndex("dbo.client", "name", unique: true, name: "index1");
        }
        
        public override void Down()
        {
            //DropIndex("dbo.client", "index1");
        }
    }
}
