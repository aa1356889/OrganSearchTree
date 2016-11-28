namespace OrganTree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DeptId = c.Int(nullable: false, identity: true),
                        OrganId = c.String(),
                        RecordID = c.String(),
                        LevelCode = c.String(),
                        DeptCode = c.String(),
                        DeptName = c.String(),
                        ParentId = c.String(),
                        SortNum = c.Double(nullable: false),
                        Tel = c.String(maxLength: 50),
                        Fax = c.String(),
                        QQ = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        District = c.String(),
                        Address = c.String(),
                        Master = c.String(),
                        DeputyMaster = c.String(),
                        Leader = c.String(),
                        Intro = c.String(),
                        Virtual = c.String(),
                        Status = c.Int(nullable: false),
                        IsDelete = c.String(),
                        TotalFavories = c.Int(nullable: false),
                        TotalViews = c.Int(nullable: false),
                        Area = c.String(),
                        TotalScheme = c.Int(nullable: false),
                        TotalComment = c.Int(nullable: false),
                        Recommended = c.Int(nullable: false),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.DeptId);
            
            CreateTable(
                "dbo.Organs",
                c => new
                    {
                        OrganId = c.Int(nullable: false, identity: true),
                        RecordID = c.String(),
                        RootOrganId = c.String(),
                        ParentId = c.String(),
                        LevelCode = c.String(),
                        OrganName = c.String(),
                        ShortName = c.String(),
                        Status = c.Int(nullable: false),
                        IsDelete = c.String(),
                        ShortChar = c.String(),
                        IsHeadquarters = c.String(),
                        IsEnable = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        EnableDateStart = c.DateTime(),
                        EnableDateEnd = c.DateTime(),
                        IsSupplier = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        District = c.String(),
                        OfficeWebsite = c.String(),
                        Website = c.String(),
                        Path = c.String(),
                        OrganCode = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.OrganId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Organs");
            DropTable("dbo.Departments");
        }
    }
}
