namespace GeekWear.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyProject : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Transform", c => c.String(nullable: false));
            AlterColumn("dbo.Projects", "ShirtColor", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "ShirtColor", c => c.String());
            AlterColumn("dbo.Projects", "Transform", c => c.String());
        }
    }
}
