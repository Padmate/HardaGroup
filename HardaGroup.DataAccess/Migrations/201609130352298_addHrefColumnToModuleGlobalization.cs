namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addHrefColumnToModuleGlobalization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ModuleGlobalizations", "Href", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ModuleGlobalizations", "Href");
        }
    }
}
