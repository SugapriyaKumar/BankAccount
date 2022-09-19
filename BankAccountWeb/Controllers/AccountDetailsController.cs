using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankAccount.Context;
using BankAccount.Models;
using BankAccount.ViewModel;
using log4net;

namespace BankAccount.Controllers
{
    public class AccountDetailsController : Controller
    {
        private dbUser db = new dbUser();
        private static readonly ILog Log = LogManager.GetLogger(typeof(AccountDetailsController));
       
        public ActionResult OnAccountCreationFailure(int id)
        {
            try {
                ViewBag.Message = "No Parent Account found with the given account number";
                User user = db.Users.Find(id);
                Log.Info("Logging test :" + user.UserID.ToString());
                db.Users.Remove(user);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error("log4net Error Level", ex);
            }                       
            return View("OnAccountCreationFailure");
        }

        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: AccountDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParentAccountNumber,CanWithdraw,WithDrawLimit")] AccountDetailViewModel accountDetailViewModel)
        {
            int userID = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    userID = Convert.ToInt32(TempData["UserID"]);
                    AccountDetail accountDetail = new AccountDetail()
                    {
                        AccountID= Convert.ToInt32(TempData["AccountID"]),
                        ParentAcccountID=db.Accounts.FirstOrDefault(a=>a.AccountNumber==accountDetailViewModel.ParentAccountNumber).AccountID,
                        CanWithdraw=accountDetailViewModel.CanWithdraw,
                        WithDrawLimit=accountDetailViewModel.WithDrawLimit
                    };
                    db.AccountDetails.Add(accountDetail);
                    Account thisAccount = db.Accounts.Find(accountDetail.AccountID);
                    Account parentAccount = db.Accounts.FirstOrDefault(a => a.AccountID == accountDetail.ParentAcccountID);

                    //Parent account number should be a valid existing account number
                    //should not be same as the current account number or another child account number
                    if(accountDetail.ParentAcccountID!=0&&
                        accountDetailViewModel.ParentAccountNumber != thisAccount.AccountNumber && 
                        parentAccount.AccountType!=Enums.AccountTypes.Child)
                    {
                        //Changing Account type to parent
                        db.Accounts.FirstOrDefault(a => a.AccountID == 
                                                accountDetail.ParentAcccountID).AccountType = Enums.AccountTypes.Parent;
                        //Changing Account type to child
                        db.Accounts.FirstOrDefault(a => a.AccountID == 
                                                accountDetail.AccountID).AccountType = Enums.AccountTypes.Child;
                        db.SaveChanges();
                        return RedirectToAction("OnAccountCreatedSuccess", "Users", new { id = userID });
                    }                    
                    return RedirectToAction("OnAccountCreationFailure", new { id = userID });                                       
                }
            }
            catch(Exception ex)
            {
                Log.Error("log4net Error Level", ex);
                return RedirectToAction("OnAccountCreationFailure", new { id = userID });
            }
            return View(accountDetailViewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
