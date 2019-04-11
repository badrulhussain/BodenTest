using System;
using App;
using App.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomerServiceTest.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private ICustomerService getTestObject()
        {
            return new CustomerService();
        }

        [TestMethod]
        public void TestMethod1()
        {
            //arrange
            ICustomerService target = getTestObject();            
            var Customer = new Customer()
            {
                Firstname = "Bruce",
                Surname = "Wayne",
                EmailAddress = "Bruce.Wayne@WayneEnterprises.com",
                DateOfBirth = new DateTime(1939, 3, 1),
                Company = new Company()
                {
                    Id = 1
                }
            };

            //act
            var customerSerive = target.AddCustomer(Customer);

            //assert
            Assert.IsTrue(customerSerive, "Customer Added");
        }
    }
}
