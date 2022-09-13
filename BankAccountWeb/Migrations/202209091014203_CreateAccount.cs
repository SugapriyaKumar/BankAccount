namespace BankAccount.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountID = c.Int(nullable: false, identity: true),
                        AccountNumber = c.Int(nullable: false),
                        AccountType = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        CreatedTimeStamp = c.DateTime(nullable: false),
                        ModifiedTimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AccountID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Accounts");
        }
    }
}
