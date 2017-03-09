namespace GeekWear.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        OrderDate = c.DateTimeOffset(precision: 7),
                        TotalCost = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Projects", "Order_Id", c => c.Int());
            CreateIndex("dbo.Projects", "Order_Id");
            AddForeignKey("dbo.Projects", "Order_Id", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "Order_Id" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropColumn("dbo.Projects", "Order_Id");
            DropTable("dbo.Orders");
        }
    }
}
