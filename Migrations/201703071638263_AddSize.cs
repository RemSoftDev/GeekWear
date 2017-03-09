namespace GeekWear.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Size", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Size");
        }
    }
}
