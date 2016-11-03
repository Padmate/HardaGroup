namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinkHrefToImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "LinkHref", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "LinkHref");
        }
    }
}
