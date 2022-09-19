using BankAccount.Context;
using BankAccount.Controllers;
using BankAccount.Models;
using BankAccount.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BankAccountTest
{
    [TestClass]
    public class AccountDetailsControllerTest
    {
        private dbUser db = new dbUser();

        [TestMethod]
        public void CreateView()
        {
            var accountDetailsController = new AccountDetailsController();
            ViewResult result = (ViewResult)accountDetailsController.Create();
            Assert.AreEqual("Create", result.ViewName);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CreateViewPost()
        {
            var accountDetailsController = new AccountDetailsController();
            UsersControllerTest uc = new UsersControllerTest();
            var user=uc.CreateTestUser();
            var usersController = new UsersController();
            ActionResult result = usersController.CreateAccount(user);
            User dbUser = db.Users.FirstOrDefault(u => u.UserName == user.UserName);
            Account account = db.Accounts.FirstOrDefault(a => a.UserID == dbUser.UserID);
            var accountDetailViewModel = new AccountDetailViewModel()
            {                
                ParentAccountNumber=23456,
                CanWithdraw=true,
                WithDrawLimit=10
            };
            RedirectToRouteResult adResult = (RedirectToRouteResult)accountDetailsController.Create(accountDetailViewModel);
            Assert.IsTrue(adResult.RouteValues["action"].ToString() == "onAccountCreationFailure");
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            uc.DeleteTestUser();
        }

        [TestMethod]
        public void onAccountCreationFailureView()
        {
            var usersController = new UsersController();
            var accountDetailsController = new AccountDetailsController();
            var userControllerTest = new UsersControllerTest();
            var userView = userControllerTest.CreateTestUser();
            ActionResult resultUser = usersController.CreateAccount(userView);
            userView.UserID = db.Users.FirstOrDefault(u => u.UserName == userView.UserName).UserID;
            ViewResult result = (ViewResult)accountDetailsController.OnAccountCreationFailure(userView.UserID);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("onAccountCreationFailure", result.ViewName);
            Assert.AreEqual("No Parent Account found with the given account number",result.ViewData["Message"].ToString());
        }
    }
}
