using BankAccount.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankAccount.Models
{
    public class Account
    {
        
        [Key]
        public int AccountID { get; set; }

        [Index(IsUnique = true)]
        public int AccountNumber { get; set; }
        public AccountTypes AccountType { get; set; }  
        
        public int UserID { get; set; }

        public DateTime CreatedTimeStamp { get; set; }
        public DateTime ModifiedTimeStamp { get; set; }

        [ForeignKey(nameof(UserID))]
        public User User { get; set; }
    }
}