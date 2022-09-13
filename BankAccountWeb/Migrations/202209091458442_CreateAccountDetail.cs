namespace BankAccount.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAccountDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountDetails",
                c => new
                    {
                        AccountDetailID = c.Int(nullable: false, identity: true),
                        AccountID = c.Int(nullable: false),
                        ParentAcccountNumber = c.Int(nullable: false),
                        CanWithdraw = c.Boolean(nullable: false),
                        WithDrawLimit = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.AccountDetailID)
                .ForeignKey("dbo.Accounts", t => t.AccountID, cascadeDelete: true)
                .Index(t => t.AccountID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountDetails", "AccountID", "dbo.Accounts");
            DropIndex("dbo.AccountDetails", new[] { "AccountID" });
            DropTable("dbo.AccountDetails");
        }
    }
}
