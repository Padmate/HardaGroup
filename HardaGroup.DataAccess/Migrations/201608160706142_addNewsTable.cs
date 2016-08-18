namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(),
                        SubTitle = c.String(),
                        Description = c.String(),
                        Type = c.String(),
                        Content = c.String(storeType: "ntext"),
                        ImageId = c.Int(),
                        Creator = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Modifier = c.String(),
                        ModifiedDate = c.DateTime(),
                        Pubtime = c.DateTime(nullable: false),
                        Culture = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.News");
        }
    }
}
