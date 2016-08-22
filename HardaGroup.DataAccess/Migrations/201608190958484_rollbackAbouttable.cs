namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rollbackAbouttable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AboutGlobalizations", "AboutId", "dbo.Abouts");
            DropIndex("dbo.AboutGlobalizations", new[] { "AboutId" });
            AddColumn("dbo.Abouts", "TypeName", c => c.String(maxLength: 200));
            AddColumn("dbo.Abouts", "Content", c => c.String(storeType: "ntext"));
            AddColumn("dbo.Abouts", "Culture", c => c.String(maxLength: 50));
            DropTable("dbo.AboutGlobalizations");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Abouts", "Culture");
            DropColumn("dbo.Abouts", "Content");
            DropColumn("dbo.Abouts", "TypeName");
            CreateIndex("dbo.AboutGlobalizations", "AboutId");
            AddForeignKey("dbo.AboutGlobalizations", "AboutId", "dbo.Abouts", "Id", cascadeDelete: true);
        }
    }
}
