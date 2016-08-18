namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewsScope : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsScopes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TypeCode = c.String(maxLength: 100),
                        TypeName = c.String(maxLength: 200),
                        Sequence = c.Int(nullable: false),
                        Culture = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.News", "NewsScopeId", c => c.Guid(nullable: false));
            AlterColumn("dbo.News", "Description", c => c.String(maxLength: 200));
            CreateIndex("dbo.News", "NewsScopeId");
            AddForeignKey("dbo.News", "NewsScopeId", "dbo.NewsScopes", "Id", cascadeDelete: true);
            DropColumn("dbo.News", "Type");
            DropColumn("dbo.News", "Culture");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "Culture", c => c.String(maxLength: 50));
            AddColumn("dbo.News", "Type", c => c.String());
            DropForeignKey("dbo.News", "NewsScopeId", "dbo.NewsScopes");
            DropIndex("dbo.News", new[] { "NewsScopeId" });
            AlterColumn("dbo.News", "Description", c => c.String());
            DropColumn("dbo.News", "NewsScopeId");
            DropTable("dbo.NewsScopes");
        }
    }
}
