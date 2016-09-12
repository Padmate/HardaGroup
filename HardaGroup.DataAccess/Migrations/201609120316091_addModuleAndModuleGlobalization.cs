namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addModuleAndModuleGlobalization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModuleGlobalizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        SubTitle = c.String(),
                        Description = c.String(maxLength: 200),
                        Content = c.String(storeType: "ntext"),
                        ImageId = c.Int(),
                        ImageClass = c.String(),
                        Culture = c.String(maxLength: 50),
                        ModuleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .Index(t => t.ModuleId);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuleURLId = c.String(maxLength: 200),
                        Sequence = c.Int(nullable: false),
                        Type = c.String(maxLength: 200),
                        Creator = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Modifier = c.String(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModuleGlobalizations", "ModuleId", "dbo.Modules");
            DropIndex("dbo.ModuleGlobalizations", new[] { "ModuleId" });
            DropTable("dbo.Modules");
            DropTable("dbo.ModuleGlobalizations");
        }
    }
}
