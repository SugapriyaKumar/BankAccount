using BankAccount.Context;
using BankAccount.Controllers;
using BankAccount.Models;
using BankAccount.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BankAccountTest
{
    [TestClass]
    public class UsersControllerTest
    {
        private dbUser db = new dbUser();
        private string testUserName = "TestUser123";
        /// <summary>    
        /// This method used for index view    
        /// </summary>    
        [TestMethod]
        public void IndexView()
        {
            var usersController = new UsersController();
            ViewResult result = (ViewResult)usersController.Index();
            Assert.AreEqual("Index", result.ViewName);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        /// <summary>    
        /// This method used for CreateAccount view    
        /// </summary>    
        [TestMethod]
        public void CreateAccountView()
        {
            var usersController = new UsersController();
            ViewResult result = (ViewResult)usersController.CreateAccount();
            Assert.AreEqual("CreateAccount", result.ViewName);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }


        [TestMethod]
        public void CreateAccountViewPost()
        {
            int initialUserCount = db.Users.Count();
            var usersController = new UsersController();
            var user = CreateTestUser();
            ActionResult result = usersController.CreateAccount(user);
            int updatedUserCount = db.Users.Count();
            Assert.IsNotNull(result);
            Assert.AreEqual(initialUserCount + 1, updatedUserCount);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            DeleteTestUser();
        }
        /// <summary>    
        /// This method used for Details view    
        /// </summary>    
        [TestMethod]
        public void DetailsView()
        {
            var usersController = new UsersController();
            HttpNotFoundResult notFoundResult = (HttpNotFoundResult)usersController.Details(0);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.IsInstanceOfType(notFoundResult, typeof(HttpNotFoundResult));
            var user = db.Users.FirstOrDefault();
            if (user != null)
            {
                ViewResult result = (ViewResult)usersController.Details(user.UserID);
                Assert.AreEqual("Details", result.ViewName);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }
            else
            {
                HttpStatusCodeResult badRequestResult = (HttpStatusCodeResult)usersController.Details(null);
                Assert.AreEqual(400, badRequestResult.StatusCode);
                Assert.IsInstanceOfType(badRequestResult, typeof(HttpStatusCodeResult));
            }

        }

        /// <summary>    
        /// This method used for Edit view    
        /// </summary>    
        [TestMethod]
        public void EditView()
        {
            var usersController = new UsersController();
            User user = db.Users.FirstOrDefault();
            if (user == null)
            {
                HttpStatusCodeResult badRequestResult = (HttpStatusCodeResult)usersController.Edit(0);
                Assert.AreEqual(400, badRequestResult.StatusCode);
                Assert.IsInstanceOfType(badRequestResult, typeof(HttpStatusCodeResult));
            }
            else
            {
                ViewResult result = (ViewResult)usersController.Edit(user.UserID);
                Assert.AreEqual("Edit", result.ViewName);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }

        }

        [TestMethod]
        public void EditViewPost()
        {
            var usersController = new UsersController();
            var userView = CreateTestUser();
            ActionResult resultUser = usersController.CreateAccount(userView);
            userView.UserID = db.Users.FirstOrDefault(u => u.UserName == userView.UserName).UserID;
            RedirectToRouteResult result = (RedirectToRouteResult)usersController.Edit(userView);
            Assert.IsTrue(result.RouteValues["action"].ToString() == "onAccountCreatedSuccess");
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            DeleteTestUser();
        }

        [TestMethod]
        public void DeleteView()
        {
            var usersController = new UsersController();
            var userView = CreateTestUser();
            ActionResult resultUser = usersController.CreateAccount(userView);
            userView.UserID = db.Users.FirstOrDefault(u => u.UserName == userView.UserName).UserID;
            ViewResult result = (ViewResult)usersController.Delete(userView.UserID);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("Delete", result.ViewName);
            DeleteTestUser();
        }

        [TestMethod]
        public void DeleteViewPost()
        {
            var usersController = new UsersController();
            var userView = CreateTestUser();
            ActionResult resultUser = usersController.CreateAccount(userView);
            userView.UserID = db.Users.FirstOrDefault(u => u.UserName == userView.UserName).UserID;
            RedirectToRouteResult result = (RedirectToRouteResult)usersController.DeleteConfirmed(userView.UserID);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
            User user = db.Users.FirstOrDefault(u => u.UserID == userView.UserID);
            Assert.IsNull(user);
        }

        public UserViewModel CreateTestUser()
        {
            UserViewModel user = new UserViewModel()
            {
                UserName = testUserName,
                FirstName = "FN",
                LastName = "LN",
                Age = 29,
                DateOfBirth = Convert.ToDateTime("09/02/1993"),
                AccountNumber = 78544354,
                AccountType = BankAccount.Enums.AccountTypes.Normal,
                CreatedTimeStamp = DateTime.Now
            };
            return user;
        }

        public void DeleteTestUser()
        {
            User dbUser = db.Users.First(u => u.UserName == testUserName);
            db.Users.Remove(dbUser);
            db.SaveChanges();
        }
    }
}


