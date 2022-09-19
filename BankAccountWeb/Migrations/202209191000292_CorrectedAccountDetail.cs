namespace BankAccount.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectedAccountDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountDetails", "ParentAcccountID", c => c.Int(nullable: false));
            DropColumn("dbo.AccountDetails", "ParentAcccountNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccountDetails", "ParentAcccountNumber", c => c.Int(nullable: false));
            DropColumn("dbo.AccountDetails", "ParentAcccountID");
        }
    }
}
