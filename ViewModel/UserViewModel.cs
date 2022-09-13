using BankAccount.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankAccount.ViewModel
{
    public class UserViewModel
    {
        [DisplayName("User ID")]
        public int UserID { get; set; }
        [DisplayName("User Name")]
        [Required]
        public string UserName { get; set; }
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Date of Birth")]
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]        
        public DateTime DateOfBirth { get; set; }
        [DisplayName("Created Timestamp")]
        public DateTime CreatedTimeStamp { get; set; }
        [DisplayName("Age")]
        public int Age { get; set; }
        [DisplayName("Account Number")]
        public int AccountNumber { get; set; }

        [DisplayName("Account Type")]
        [Required]
        public AccountTypes AccountType { get; set; }
    }
}