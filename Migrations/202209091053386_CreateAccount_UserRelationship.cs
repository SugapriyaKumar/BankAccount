namespace BankAccount.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAccount_UserRelationship : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Accounts", "UserID");
            AddForeignKey("dbo.Accounts", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "UserID", "dbo.Users");
            DropIndex("dbo.Accounts", new[] { "UserID" });
        }
    }
}
