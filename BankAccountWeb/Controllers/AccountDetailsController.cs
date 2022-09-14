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
using log4net;

namespace BankAccount.Controllers
{
    public class AccountDetailsController : Controller
    {
        private dbUser db = new dbUser();
        private static readonly ILog Log = LogManager.GetLogger(typeof(AccountDetailsController));
       
        public ActionResult onAccountCreationFailure(int id)
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
            return View("onAccountCreationFailure");
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
        public ActionResult Create([Bind(Include = "AccountDetailID,AccountID,ParentAcccountNumber,CanWithdraw,WithDrawLimit")] AccountDetail accountDetail)
        {
            int userID = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    userID = Convert.ToInt32(TempData["UserID"]);
                    accountDetail.AccountID = Convert.ToInt32(TempData["AccountID"]);
                    db.AccountDetails.Add(accountDetail);
                    Account thisAccount = db.Accounts.Find(accountDetail.AccountID);
                    Account parentAccount = db.Accounts.FirstOrDefault(a => a.AccountNumber == accountDetail.ParentAcccountNumber);

                    //Parent account number should be a valid existing account number
                    //should not be same as the current account number or another child account number
                    if(db.Accounts.Any(a=>a.AccountNumber==accountDetail.ParentAcccountNumber)&&
                        accountDetail.ParentAcccountNumber!= thisAccount.AccountNumber && 
                        parentAccount.AccountType!=Enums.AccountTypes.Child)
                    {
                        //Changing Account type to parent
                        db.Accounts.FirstOrDefault(a => a.AccountNumber == 
                                                accountDetail.ParentAcccountNumber).AccountType = Enums.AccountTypes.Parent;
                        //Changing Account type to child
                        db.Accounts.FirstOrDefault(a => a.AccountID == 
                                                accountDetail.AccountID).AccountType = Enums.AccountTypes.Child;
                        db.SaveChanges();
                        return RedirectToAction("onAccountCreatedSuccess", "Users", new { id = userID });
                    }                    
                    return RedirectToAction("onAccountCreationFailure", new { id = userID });                                       
                }
                ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "AccountID", accountDetail.AccountID);
            }
            catch(Exception ex)
            {
                Log.Error("log4net Error Level", ex);
                return RedirectToAction("onAccountCreationFailure", new { id = userID });
            }
            return View(accountDetail);
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
