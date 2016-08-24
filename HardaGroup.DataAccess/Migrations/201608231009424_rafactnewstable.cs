namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rafactnewstable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsGlobalizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        SubTitle = c.String(),
                        Description = c.String(maxLength: 200),
                        Content = c.String(storeType: "ntext"),
                        ImageId = c.Int(),
                        Culture = c.String(maxLength: 50),
                        NewsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.News", t => t.NewsId, cascadeDelete: true)
                .Index(t => t.NewsId);
            
            AddColumn("dbo.News", "NewsURLId", c => c.String(maxLength: 200));
            DropColumn("dbo.News", "Title");
            DropColumn("dbo.News", "SubTitle");
            DropColumn("dbo.News", "Description");
            DropColumn("dbo.News", "Content");
            DropColumn("dbo.News", "ImageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "ImageId", c => c.Int());
            AddColumn("dbo.News", "Content", c => c.String(storeType: "ntext"));
            AddColumn("dbo.News", "Description", c => c.String(maxLength: 200));
            AddColumn("dbo.News", "SubTitle", c => c.String());
            AddColumn("dbo.News", "Title", c => c.String());
            DropForeignKey("dbo.NewsGlobalizations", "NewsId", "dbo.News");
            DropIndex("dbo.NewsGlobalizations", new[] { "NewsId" });
            DropColumn("dbo.News", "NewsURLId");
            DropTable("dbo.NewsGlobalizations");
        }
    }
}
