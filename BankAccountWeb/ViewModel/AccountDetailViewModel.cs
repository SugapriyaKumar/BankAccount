using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankAccount.ViewModel
{
    public class AccountDetailViewModel
    {
        [DisplayName("Parent Account Number")]
        [Required]
        public int ParentAccountNumber { get; set; }

        [DisplayName("Can Withdraw?")]
        [Required]
        public bool CanWithdraw { get; set; }

        [DisplayName("Withdraw Limit")]
        [Required]
        public double WithDrawLimit { get; set; }

    }
}