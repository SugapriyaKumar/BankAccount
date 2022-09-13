using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankAccount.Context;
using BankAccount.Helpers;
using BankAccount.Models;
using BankAccount.ViewModel;
using log4net;


namespace BankAccount.Controllers
{
    public class UsersController : Controller
    {
        private dbUser db = new dbUser();
        private static readonly ILog Log = LogManager.GetLogger(typeof(UsersController));

        // GET: Users
        public ActionResult Index()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            try
            {
                users = db.Users.Include(d => d.Accounts).ToList().Select(u => new UserViewModel
                {
                    UserID = u.UserID,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    Age = AgeCalculator.CalculateAge(u.DateOfBirth),
                    CreatedTimeStamp = u.CreatedTimeStamp,
                    AccountNumber = (int)u?.Accounts?.FirstOrDefault(a => a.UserID == u.UserID)?.AccountNumber,
                    AccountType = (Enums.AccountTypes)(u?.Accounts?.FirstOrDefault(a => a.UserID == u.UserID)?.AccountType)
                }).ToList();
            }
             catch(Exception ex)
            {
                Log.Error("log4net Error Level", ex);
            }
            return View("Index",users);
        }

        public ActionResult onAccountCreatedSuccess(int? id)
        {
            var userViewModel = new UserViewModel();
            try 
            { 
            User user = db.Users.Find(id);
            Account account = db.Accounts.FirstOrDefault(a => a.UserID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            userViewModel = new UserViewModel
            {
                UserID = user.UserID,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Age = AgeCalculator.CalculateAge(user.DateOfBirth),
                CreatedTimeStamp = user.CreatedTimeStamp,
                AccountNumber = account.AccountNumber
            };
            }
            catch (Exception ex)
            {
                Log.Error("log4net Error Level", ex);
            }
            return View(userViewModel);
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            User user = new User();
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error("log4net Error Level", ex);
            }            
            return View("Details",user);
        }

        // GET: Users/Create
        public ActionResult CreateAccount()
        {
            ViewBag.Message = false;
            return View("CreateAccount");
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount([Bind(Include = "UserID,UserName,FirstName,LastName,DateOfBirth,AccountType")] UserViewModel userView)
        {
            int age = 0;
            ViewBag.Message = false;
            try {
                if (ModelState.IsValid)
                {
                    age = AgeCalculator.CalculateAge(userView.DateOfBirth);

                    if ((age < 18 && userView.AccountType != Enums.AccountTypes.Child) ||
                        (age >= 18 && userView.AccountType == Enums.AccountTypes.Child))
                    {
                        TempData["WarningMessage"] = "Child account is applicable for those under 18." +
                            " Please select appropriate account type";
                        ViewBag.Message = true;
                        return View(userView);
                    }
                    User user = new User()
                    {
                        UserID = RandomNumberGenerator.PinGenerator(5),
                        CreatedTimeStamp = DateTime.Now,
                        UserName = userView.UserName,
                        FirstName = userView.FirstName,
                        LastName = userView.LastName,
                        DateOfBirth = userView.DateOfBirth
                    };

                    var account = new Account()
                    {
                        AccountNumber = RandomNumberGenerator.PinGenerator(10),
                        AccountType = userView.AccountType,
                        UserID = user.UserID,
                        CreatedTimeStamp = user.CreatedTimeStamp,
                        ModifiedTimeStamp = user.CreatedTimeStamp
                    };

                    db.Users.Add(user);
                    db.Accounts.Add(account);
                    db.SaveChanges();


                    if (age >= 18)
                    {
                        return RedirectToAction("onAccountCreatedSuccess", new { id = user.UserID });
                    }
                    else
                    {
                        TempData["UserID"] = user.UserID;
                        TempData["AccountID"] = db.Accounts.FirstOrDefault(a => a.UserID == user.UserID).AccountID;
                        return RedirectToAction("Create", "AccountDetails", "");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("log4net Error Level", ex);
            }
            return View(userView);
        }
        

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Message = false;
            var userViewModel = new UserViewModel();
            try {
                if (id == null||id==0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(id);
                Account account = db.Accounts.FirstOrDefault(a => a.UserID == user.UserID);
                if (user == null)
                {
                    return HttpNotFound();
                }
                userViewModel = new UserViewModel()
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    AccountType = account.AccountType,
                    AccountNumber = account.AccountNumber,
                    Age = AgeCalculator.CalculateAge(user.DateOfBirth)
                };
                
            }
            catch (Exception ex)
            {
                Log.Error("log4net Error Level", ex);
            }            
            return View("Edit",userViewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,FirstName,LastName,DateOfBirth,CreatedTimeStamp,AccountType")] UserViewModel userView)
        {
            try {
                int age = 0;
                ViewBag.Message = false;

                if (ModelState.IsValid)
                {
                    age = AgeCalculator.CalculateAge(userView.DateOfBirth);

                    if ((age < 18 && userView.AccountType != Enums.AccountTypes.Child) ||
                        (age >= 18 && userView.AccountType == Enums.AccountTypes.Child))
                    {
                        TempData["WarningMessage"] = "Child account is applicable for those under 18." +
                            " Please select appropriate account type";
                        ViewBag.Message = true;
                        return View(userView);
                    }
                    var dbUser = db.Users.Find(userView.UserID);
                    dbUser.FirstName = userView.FirstName;
                    dbUser.LastName = userView.LastName;
                    dbUser.UserName = userView.UserName;
                    dbUser.DateOfBirth = userView.DateOfBirth;
                    var dbAccount = db.Accounts.FirstOrDefault(u => u.UserID == userView.UserID);
                    dbAccount.ModifiedTimeStamp = DateTime.Now;
                    dbAccount.AccountType = userView.AccountType;
                    db.SaveChanges();

                    if (age < 18)
                    {
                        TempData["UserID"] = userView.UserID;
                        TempData["AccountID"] = db.Accounts.FirstOrDefault(a => a.UserID == userView.UserID).AccountID;
                        return RedirectToAction("Create", "AccountDetails", "");
                    }
                    if (age >= 18)
                    {
                        var accountDetail = db.AccountDetails.FirstOrDefault(a => a.AccountID == dbAccount.AccountID);
                        if (accountDetail != null)
                        {
                            db.AccountDetails.Remove(accountDetail);
                            db.SaveChanges();
                        }
                    }
                    
                    return RedirectToAction("onAccountCreatedSuccess", new { id = userView.UserID });
                }
            }
            catch (Exception ex)
            {
                Log.Error("log4net Error Level", ex);
            }
            return View("Edit",userView);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            User user = new User();
            try {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error("log4net Error Level", ex);
            }
            return View("Delete",user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try {

                User user = db.Users.Find(id);
                db.Users.Remove(user);
                Account thisAccount = db.Accounts.FirstOrDefault(a => a.UserID == user.UserID);
                if (thisAccount.AccountType == Enums.AccountTypes.Parent)
                {
                    AccountDetail ChildAccountDetail = db.AccountDetails.FirstOrDefault(a => a.ParentAcccountNumber == thisAccount.AccountNumber);
                    Account childAccount = new Account();
                    User childUser = new User();
                    if (ChildAccountDetail != null)
                    {
                        childAccount = db.Accounts.Find(ChildAccountDetail.AccountID);
                    }
                    if (childAccount != null)
                    {
                        childUser = db.Users.Find(childAccount.UserID);
                    }
                    if (childUser != null)
                    {
                        db.Users.Remove(childUser);
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error("log4net Error Level", ex);
            }
            return RedirectToAction("Index");
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
