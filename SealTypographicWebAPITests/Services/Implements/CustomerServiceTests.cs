using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SealTypographicWebAPI.Config;
using DBEntities;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Services.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SealTypographicWebAPITests.Services.Implements
{
    [TestClass()]
    public class CustomerServiceTests
    {
        //private readonly DbContextOptions<SealTypographicDbContext> dbContext;
        private readonly CustomerService customerService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="customerService">管理客戶資料的Service</param>
        public CustomerServiceTests(CustomerService customerService)
        {
            //this.customerService = customerService;            

            //string dbName = $"AuthorPostsDb_{DateTime.Now.ToFileTimeUtc()}";
            //dbContext = new DbContextOptionsBuilder<SealTypographicDbContext>()
            //                .UseInMemoryDatabase(dbName)
            //                .Options;
            this.customerService = customerService;
            //IMapper mapper = new Mock<Mapper>();
            //customerService = new CustomerService(new SealTypographicDbContext(dbContext), mapper);
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