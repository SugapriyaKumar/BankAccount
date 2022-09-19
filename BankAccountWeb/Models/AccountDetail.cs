using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankAccount.Models
{
    public class AccountDetail
    {
        [Key]
        public int AccountDetailID { get; set; }
        public int AccountID { get; set; }
        public int ParentAcccountID { get; set; }
        public bool CanWithdraw { get; set; }
        public double WithDrawLimit { get; set; }

        [ForeignKey(nameof(AccountID))]
        public Account Account { get; set; }
    }
}