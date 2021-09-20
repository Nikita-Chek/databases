namespace lab12_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.client",
            //    c => new
            //        {
            //            id = c.Int(nullable: false, identity: true),
            //            name = c.String(nullable: false, unicode: false),
            //        })
            //    .PrimaryKey(t => t.id);
            
            //CreateTable(
            //    "dbo.manufactory",
            //    c => new
            //        {
            //            id = c.Int(nullable: false, identity: true),
            //            name = c.String(nullable: false, unicode: false),
            //            coutry = c.String(nullable: false, unicode: false),
            //        })
            //    .PrimaryKey(t => t.id);
            
            //CreateTable(
            //    "dbo.product",
            //    c => new
            //        {
            //            id = c.Int(nullable: false, identity: true),
            //            name = c.String(nullable: false, unicode: false),
            //            type = c.String(unicode: false),
            //            quantity = c.Int(nullable: false),
            //            manuf_id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.id)
            //    .ForeignKey("dbo.manufactory", t => t.manuf_id, cascadeDelete: true)
            //    .Index(t => t.manuf_id);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.product", "manuf_id", "dbo.manufactory");
            //DropIndex("dbo.product", new[] { "manuf_id" });
            //DropTable("dbo.product");
            //DropTable("dbo.manufactory");
            //DropTable("dbo.client");
        }
    }
}
