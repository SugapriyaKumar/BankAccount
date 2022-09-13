using BankAccount.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BankAccount.Context
{
    public class dbUser : DbContext
    {
        public dbUser():base("name=DBConnect")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountDetail> AccountDetails { get; set; }

    }
}