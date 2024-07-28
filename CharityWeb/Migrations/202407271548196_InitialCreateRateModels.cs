namespace CharityWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreateRateModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RateModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Feedback = c.String(nullable: false),
                        SubmittedUser = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Feedback");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Feedback",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        FeedbackText = c.String(nullable: false),
                        SubmittedUsername = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.RateModels");
        }
    }
}
