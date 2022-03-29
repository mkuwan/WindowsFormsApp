using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WindowsFormsApp.Domain.DapperRepository;
using WindowsFormsApp.Domain.Events;
using WindowsFormsApp.Domain.Models;
using WindowsFormsApp.Infrastructure.DapperRepository;
using WindowsFormsApp.Infrastructure.DBConnection;

namespace UnitTestProject
{
    [TestClass]
    public class CustomerRepositoryTest
    {
        private static Mock<WinFormDbConnectionStringConfig> mockConnectionConfig;
        private static ICustomerRepository _customerRepository;

        private static string connectionString = "Data Source=localhost,1433;Initial Catalog=winformDb;Persist Security Info=False;User ID=sa;Password=aspMVC98;Pooling=False;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=False";
        private static Guid Id = Guid.Empty;
        private static string Name = "testName";

        [ClassInitialize]
        public static async Task ClassInitAsync(TestContext context)
        {
            mockConnectionConfig = new Moq.Mock<WinFormDbConnectionStringConfig>();
            //mockConnectionConfig.SetupAllProperties();
            mockConnectionConfig.SetupProperty(x => x.SqlServerConnectionString, connectionString);

            _customerRepository = new CustomerRepository(mockConnectionConfig.Object);

            CustomerEntity customerEntity = new CustomerEntity(Guid.Empty, Name);
            var c = await _customerRepository.UpdateCustomer(customerEntity);
            Id = c.Id;
        }

        [TestInitialize]
        public void Init()
        {
            Console.WriteLine(@"TestInitialize");
        }

        [ClassCleanup]
        public static async Task Cleanup()
        {
            await _customerRepository.DeleteCustomer(Id);
        }

        [TestMethod("Check Init")]
        public void CheckInit()
        {
            Assert.AreEqual(connectionString, mockConnectionConfig.Object.SqlServerConnectionString);
        }

        [TestMethod("GetCustomer")]
        public async Task GetCustomerTest()
        {

            CustomerEntity customerEntity = new CustomerEntity(Guid.NewGuid(),  "guest");

            var repo = new Mock<ICustomerRepository>();
            repo.Setup(x => x.GetCustomerById(It.IsAny<Guid>()))
                .ReturnsAsync(customerEntity)
                .Verifiable();

            var result = await repo.Object.GetCustomerById(customerEntity.Id);

            Assert.AreEqual(customerEntity, result);
            repo.Verify(x => x.GetCustomerById(customerEntity.Id), Times.Once);
        }

        [TestMethod("GetCustomer_from_db")]
        public async Task GetCustomerFromDbTest()
        {
            var customer = await _customerRepository.GetCustomerById(Id);

            Assert.AreEqual(Name, customer.Name);
        }



        private string CustmerName = string.Empty;
        [TestMethod("Domain Event use CustomerDomainEventHandler")]
        public void CustomerDomainHandlerEventTest()
        {
            // Arrange
            CustomerEntity customerEntity = new CustomerEntity(Guid.NewGuid(), "DomainCustomer");
            
            CustomerDomainEventHandler handler = new CustomerDomainEventHandler();
            handler.OnHandled += Handled;
            
            // Act
            customerEntity.ChangeName("newName");

            var events = customerEntity.DomainEvent;
            foreach (var domainEvent in events)
            {
                if(domainEvent.GetType() == typeof(CustomerDomainEvent))
                    handler.OnHandle((CustomerDomainEvent)domainEvent);
            }

            // Assertion
            Assert.AreEqual("newName", customerEntity.Name);
            Assert.AreEqual(customerEntity.Name, CustmerName);
        }

        private void Handled(object sender, CustomerDomainEvent eventArgs)
        {
            var args = eventArgs.CustomerEntity;
            CustmerName = args.Name;
        }



        private CustomerEntity _customerEntityResult = new CustomerEntity(Guid.NewGuid(), "customerEntity");
        [TestMethod("Domain Event use DomainEvents(static class)")]
        public void CustomerDomainEventsTest()
        {
            DomainEvents.Register<CustomerDomainEvent>(Handler);

            CustomerEntity customerEntity = new CustomerEntity(Guid.NewGuid(), "customerEntity");
            customerEntity.ChangeName("newName1");
            customerEntity.ChangeName("newName2");
            customerEntity.ChangeName("newName3");

            var events = customerEntity.DomainEvent;
            foreach (var domainEvent in events)
            {
                DomainEvents.Raise((CustomerDomainEvent)domainEvent);
            }
            
            Assert.AreEqual("newName3", _customerEntityResult.Name);
        }

        private void Handler(CustomerDomainEvent args)
        {
            _customerEntityResult = args.CustomerEntity;
        }
    }
}
