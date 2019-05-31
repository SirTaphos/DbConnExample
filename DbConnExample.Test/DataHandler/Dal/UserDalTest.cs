using DbConnExample.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Transactions;

namespace DbConnExample.Test.DataHandler.Dal
{
    [TestClass]
    public class UserDalTest
    {
        private UserDal _dal = new UserDal();
        [TestMethod]
        public void CreateUser()
        {
            using (var scope = new TransactionScope())
            {
                // Arrange
                Setup();
                // Act                
                var users = _dal.ReadUsers();
                var testUser = users.Where(u => u.Name.Contains("test"));
                // Assert
                Assert.IsTrue(testUser != null);
                // scope.Complete // This stops the user from being created for real
            }
        }

        [TestMethod]
        public void ReadUser()
        {
            // Act
            var users = _dal.ReadUsers();
            // Assert
            CollectionAssert.AllItemsAreUnique(users);
        }

        [TestMethod]
        public void UpdateUser()
        {
            using (var scope = new TransactionScope())
            {
                // Arrange
                Setup();
                // Act
                _dal.UpdateUser(new User { Name = "Test Updatyson", GroupId = Enums.GroupEnums.Admins });
                var users = _dal.ReadUsers();
                var testUser = users.Where(u => u.Name.Contains("test"));
                // Assert
                Assert.AreEqual(testUser.FirstOrDefault().Name, "Test Updatyson");
                // scope.Complete // This stops the user from being created for real
            }
        }

        [TestMethod]
        public void Delete()
        {
            // TODO:
        }

        [TestMethod]
        public void DoesUsernameExistFalse()
        {
            // Arrange
            var username = "admin";
            // Act
            var check = _dal.DoesUsernameExist(username);
            // Assert
            Assert.IsFalse(check);
        }

        [TestMethod]
        public void DoesUsernameExistTrue()
        {
            // Arrange
            var username = "bfp";
            // Act
            var check = _dal.DoesUsernameExist(username);
            // Assert
            Assert.IsTrue(check);
        }

        private void Setup()
        {
            var user = new User
            {
                Name = "Test Testerson",
                GroupId = Enums.GroupEnums.Readers,
                Username = "Test",
                Password = "Secret"
            };
            _dal.CreateUser(user);
        }
    }
}
