namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImageTable : DbMigration
    {
        public override void Up()
        {
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
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Images");
        }
    }
}
