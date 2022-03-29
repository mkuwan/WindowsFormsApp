using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBatisNet.DataMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WindowsFormsApp.Domain.DapperRepository;
using WindowsFormsApp.Domain.Models;
using WindowsFormsApp.Infrastructure.DBConnection;
using WindowsFormsApp.Infrastructure.MyBatisRepository;


namespace UnitTestProject
{
    /// <summary>
    /// MSTest では、テストはテスト名の順序に自動的に設定されます
    /// </summary>
    [TestClass]
    public class CustomerMyBatisRepositoryTest
    {
        private static ICustomerRepository _customerRepository;
        private static string connectionString = "Data Source=localhost,1433;Initial Catalog=winformDb;Persist Security Info=False;User ID=sa;Password=aspMVC98;Pooling=False;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=False";
        private static Guid[] Ids = new Guid[3];
        private static string TestName1 = "testName1";
        private static string TestName2 = "testName2";
        private static string TestName3 = "testName3";

        [ClassInitialize]
        public static async Task Init(TestContext context)
        {
            var mockConnectionConfig = new Mock<WinFormDbConnectionStringConfig>();
            mockConnectionConfig.SetupProperty(x => x.SqlServerConnectionString, connectionString);
            _customerRepository = new CustomerMyBatisRepository(mockConnectionConfig.Object);

            CustomerEntity customerEntity = new CustomerEntity(Guid.Empty, TestName1);
            var entity = await _customerRepository.UpdateCustomer(customerEntity);
            Ids[0] = entity.Id;
            customerEntity = new CustomerEntity(Guid.Empty, TestName2);
            entity = await _customerRepository.UpdateCustomer(customerEntity);
            Ids[1] = entity.Id;
            customerEntity = new CustomerEntity(Guid.Empty, TestName3);
            entity = await _customerRepository.UpdateCustomer(customerEntity);
            Ids[2] = entity.Id;
        }

        [ClassCleanup]
        public static async Task Cleanup()
        {
            foreach (var id in Ids)
            {
                await _customerRepository.DeleteCustomer(id);
            }
        }

        /// <summary>
        /// テストで作成したデータ削除用です
        /// </summary>
        /// <param name="id"></param>
        private void SetIdArrayForClean(Guid id)
        {
            Array.Resize(ref Ids, Ids.Length + 1);
            Ids[Ids.Length - 1] = id;
        }

        [TestMethod, Priority(1)]
        public async Task GetListTest()
        {
            // Arrange

            // Act
            var list = await _customerRepository.GetCustomers();

            // Assertion
            Assert.IsTrue(list.Count == 3);
        }


        [TestMethod]
        public async Task GetTest()
        {
            // Arrange

            // Act
            var entity = await _customerRepository.GetCustomerById(Ids[0]);

            // Assertion
            Assert.AreEqual(TestName1, entity.Name);
        }

        [TestMethod]
        public async Task InsertTest()
        {
            var name = "customer01";
            // Arrange
            CustomerEntity customerEntity = new CustomerEntity(Guid.Empty, name);

            // Act
            var entity = await _customerRepository.UpdateCustomer(customerEntity);

            // Assertion
            Assert.AreEqual(name, entity.Name);

            SetIdArrayForClean(entity.Id);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            var updatedName = "UpdatedName";
            // Arrange
            CustomerEntity customerEntity = new CustomerEntity(Ids[1], updatedName);

            // Act
            var entity = await _customerRepository.UpdateCustomer(customerEntity);

            // Assertion
            Assert.AreEqual(updatedName, entity.Name);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            // Arrange

            // Act
            await _customerRepository.DeleteCustomer(Ids[2]);

            // Assertion
            var entity = await _customerRepository.GetCustomerById(Ids[2]);
            Assert.IsNull(entity);
        }
    }
}
