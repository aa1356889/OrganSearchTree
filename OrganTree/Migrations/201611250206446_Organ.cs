namespace OrganTree.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Organ : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organs", "isgroup", c => c.String());
            AddColumn("dbo.Organs", "isreset", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organs", "isreset");
            DropColumn("dbo.Organs", "isgroup");
        }
    }
}
