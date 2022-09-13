namespace BankAccount.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUniqueKeys : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "UserName", c => c.String(maxLength: 50));
            CreateIndex("dbo.Accounts", "AccountNumber", unique: true);
            CreateIndex("dbo.Users", "UserName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.Accounts", new[] { "AccountNumber" });
            AlterColumn("dbo.Users", "UserName", c => c.String());
        }
    }
}
