namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsScrollColumnAndIsHotToNews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "IsScroll", c => c.Boolean(nullable: false));
            AddColumn("dbo.News", "IsHot", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "IsHot");
            DropColumn("dbo.News", "IsScroll");
        }
    }
}
