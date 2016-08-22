namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAbouttoGlobalization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AboutGlobalizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 200),
                        Content = c.String(storeType: "ntext"),
                        Culture = c.String(maxLength: 50),
                        AboutId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Abouts", t => t.AboutId, cascadeDelete: true)
                .Index(t => t.AboutId);
            
            DropColumn("dbo.Abouts", "TypeName");
            DropColumn("dbo.Abouts", "Content");
            DropColumn("dbo.Abouts", "Culture");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Abouts", "Culture", c => c.String(maxLength: 50));
            AddColumn("dbo.Abouts", "Content", c => c.String(storeType: "ntext"));
            AddColumn("dbo.Abouts", "TypeName", c => c.String(maxLength: 200));
            DropForeignKey("dbo.AboutGlobalizations", "AboutId", "dbo.Abouts");
            DropIndex("dbo.AboutGlobalizations", new[] { "AboutId" });
            DropTable("dbo.AboutGlobalizations");
        }
    }
}
