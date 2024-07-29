namespace CharityWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreateAppointment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NursingAppointmentModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        HomeId = c.Int(nullable: false),
                        YourDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NursingAppointmentModels");
        }
    }
}
