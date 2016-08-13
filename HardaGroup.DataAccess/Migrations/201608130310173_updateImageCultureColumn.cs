namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateImageCultureColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "Culture", c => c.String());
            DropColumn("dbo.Images", "Clture");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "Clture", c => c.String());
            DropColumn("dbo.Images", "Culture");
        }
    }
}
