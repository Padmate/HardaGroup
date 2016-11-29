namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
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
            
            CreateTable(
                "dbo.Abouts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeCode = c.String(maxLength: 100),
                        Sequence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VirtualPath = c.String(),
                        PhysicalPath = c.String(),
                        Name = c.String(),
                        SaveName = c.String(),
                        Extension = c.String(maxLength: 10),
                        Sequence = c.Int(nullable: false),
                        Type = c.String(maxLength: 50),
                        LinkHref = c.String(maxLength: 2000),
                        Culture = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.MailAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MailId = c.Int(nullable: false),
                        FileName = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Mails", t => t.MailId, cascadeDelete: true)
                .Index(t => t.MailId);
            
            CreateTable(
                "dbo.Mails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.String(),
                        To = c.String(),
                        Subject = c.String(maxLength: 2000),
                        Cc = c.String(),
                        Body = c.String(storeType: "ntext"),
                        Creator = c.String(maxLength: 50),
                        CreateDate = c.DateTime(nullable: false),
                        Modifier = c.String(maxLength: 50),
                        ModifiedDate = c.DateTime(),
                        SendDate = c.DateTime(),
                        SendTag = c.Boolean(nullable: false),
                        ReadTag = c.Boolean(nullable: false),
                        ReadDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ModuleGlobalizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        SubTitle = c.String(),
                        Description = c.String(maxLength: 200),
                        Href = c.String(),
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
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewsURLId = c.String(maxLength: 200),
                        IsScroll = c.Boolean(nullable: false),
                        IsHot = c.Boolean(nullable: false),
                        Creator = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Modifier = c.String(),
                        ModifiedDate = c.DateTime(),
                        Pubtime = c.DateTime(nullable: false),
                        NewsScopeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewsScopes", t => t.NewsScopeId, cascadeDelete: true)
                .Index(t => t.NewsScopeId);
            
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
            
            CreateTable(
                "dbo.NewsScopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeCode = c.String(maxLength: 100),
                        Sequence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsScopeGlobalizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 200),
                        Culture = c.String(maxLength: 50),
                        NewsScopeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewsScopes", t => t.NewsScopeId, cascadeDelete: true)
                .Index(t => t.NewsScopeId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.News", "NewsScopeId", "dbo.NewsScopes");
            DropForeignKey("dbo.NewsScopeGlobalizations", "NewsScopeId", "dbo.NewsScopes");
            DropForeignKey("dbo.NewsGlobalizations", "NewsId", "dbo.News");
            DropForeignKey("dbo.ModuleGlobalizations", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.MailAttachments", "MailId", "dbo.Mails");
            DropForeignKey("dbo.JobGlobalizations", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "JobScopeId", "dbo.JobScopes");
            DropForeignKey("dbo.JobScopeGlobalizations", "JobScopeId", "dbo.JobScopes");
            DropForeignKey("dbo.AboutGlobalizations", "AboutId", "dbo.Abouts");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.NewsScopeGlobalizations", new[] { "NewsScopeId" });
            DropIndex("dbo.NewsGlobalizations", new[] { "NewsId" });
            DropIndex("dbo.News", new[] { "NewsScopeId" });
            DropIndex("dbo.ModuleGlobalizations", new[] { "ModuleId" });
            DropIndex("dbo.MailAttachments", new[] { "MailId" });
            DropIndex("dbo.JobScopeGlobalizations", new[] { "JobScopeId" });
            DropIndex("dbo.Jobs", new[] { "JobScopeId" });
            DropIndex("dbo.JobGlobalizations", new[] { "JobId" });
            DropIndex("dbo.AboutGlobalizations", new[] { "AboutId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.NewsScopeGlobalizations");
            DropTable("dbo.NewsScopes");
            DropTable("dbo.NewsGlobalizations");
            DropTable("dbo.News");
            DropTable("dbo.Modules");
            DropTable("dbo.ModuleGlobalizations");
            DropTable("dbo.Mails");
            DropTable("dbo.MailAttachments");
            DropTable("dbo.JobScopeGlobalizations");
            DropTable("dbo.JobScopes");
            DropTable("dbo.Jobs");
            DropTable("dbo.JobGlobalizations");
            DropTable("dbo.Images");
            DropTable("dbo.Abouts");
            DropTable("dbo.AboutGlobalizations");
        }
    }
}
