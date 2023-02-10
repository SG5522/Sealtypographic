using Microsoft.VisualStudio.TestTools.UnitTesting;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Services.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealTypographicWebAPI.Services.Implements.Tests
{
    [TestClass()]
    public class CustomerServiceTests
    {
        /// <summary>
        /// 管理客戶資料的Service
        /// </summary>
        private readonly ICustomerService customerService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="customerService">管理客戶資料的Service</param>
        public CustomerServiceTests(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [TestMethod()]
        public void CustomerServiceTest()
        {           
            Assert.Fail();
        }

        [TestMethod()]
        public void GetDetailTest()
        {
            Assert.Fail();
        }

        [TestMethod()]        
        public void GetPaginateTest(CustomerSearch customerSearch)
        {
            customerService.GetPaginate(customerSearch);
            Assert.Fail();
        }

        [TestMethod()]
        public void NewTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }
    }
}