using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccount.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [StringLength(50)]
        [Index(IsUnique = true)] 
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedTimeStamp { get; set; }

        public List<Account> Accounts { get; set; }
    }
}