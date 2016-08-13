namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImageCltureCloumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "Clture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "Clture");
        }
    }
}
