namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobRelateTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        URLId = c.String(maxLength: 200),
                        IsHot = c.Boolean(nullable: false),
                        Creator = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Modifier = c.String(),
                        ModifiedDate = c.DateTime(),
                        Pubtime = c.DateTime(nullable: false),
                        JobScopeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobScopes", t => t.JobScopeId, cascadeDelete: true)
                .Index(t => t.JobScopeId);
            
            CreateTable(
                "dbo.JobGlobalizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobTitle = c.String(),
                        Description = c.String(maxLength: 200),
                        Location = c.String(maxLength: 200),
                        Content = c.String(storeType: "ntext"),
                        Culture = c.String(maxLength: 50),
                        JobId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .Index(t => t.JobId);
            
            CreateTable(
                "dbo.JobScopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeCode = c.String(maxLength: 100),
                        Sequence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobScopeGlobalizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 200),
                        Culture = c.String(maxLength: 50),
                        JobScopeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobScopes", t => t.JobScopeId, cascadeDelete: true)
                .Index(t => t.JobScopeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "JobScopeId", "dbo.JobScopes");
            DropForeignKey("dbo.JobScopeGlobalizations", "JobScopeId", "dbo.JobScopes");
            DropForeignKey("dbo.JobGlobalizations", "JobId", "dbo.Jobs");
            DropIndex("dbo.JobScopeGlobalizations", new[] { "JobScopeId" });
            DropIndex("dbo.JobGlobalizations", new[] { "JobId" });
            DropIndex("dbo.Jobs", new[] { "JobScopeId" });
            DropTable("dbo.JobScopeGlobalizations");
            DropTable("dbo.JobScopes");
            DropTable("dbo.JobGlobalizations");
            DropTable("dbo.Jobs");
        }
    }
}
