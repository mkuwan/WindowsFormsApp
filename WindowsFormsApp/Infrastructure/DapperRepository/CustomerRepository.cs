using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WindowsFormsApp.Domain.DapperRepository;
using WindowsFormsApp.Domain.Events;
using WindowsFormsApp.Domain.Models;
using WindowsFormsApp.Domain.SeedWork;
using WindowsFormsApp.Infrastructure.DBConnection;
using WindowsFormsApp.Infrastructure.Models;

namespace WindowsFormsApp.Infrastructure.DapperRepository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly WinFormDbConnectionStringConfig _dbConnectionStringConfig;
        private readonly CustomerDomainEventHandler handler;
        public IUnitOfWork UnitOfWork { get; }

        public CustomerRepository(WinFormDbConnectionStringConfig dbConnectionStringConfig)
        {
            _dbConnectionStringConfig = dbConnectionStringConfig;
            handler = new CustomerDomainEventHandler();
            handler.OnHandled += OnHandled;
        }

        private void Publish(CustomerEntity customerEntity)
        {
            foreach (var customerDomainEvent in customerEntity.DomainEvent)
            {
                // handler.OnHandled event を呼び出しInvokeメソッドを呼びます
                // そしてイベント購読した下のOnHandledメソッドが呼ばれます
                handler.OnHandle((CustomerDomainEvent)customerDomainEvent);
            }

            customerEntity.ClearDomainEvents();
        }

        private void OnHandled(object sender, CustomerDomainEvent eventArgs)
        {
            Console.WriteLine(eventArgs.CustomerEntity.ToString());
        }

        /// <summary>
        /// static DomainEventsを使用したイベント呼び出し
        /// クラスを切り離して実際のhandlerを定義することができます(複数でも可能)
        /// </summary>
        /// <param name="customerEntity"></param>
        private void Raise(CustomerEntity customerEntity)
        {
            foreach (var domainEvent in customerEntity.DomainEvent)
            {
                DomainEvents.Raise((CustomerDomainEvent)domainEvent);
            }

            customerEntity.ClearDomainEvents();
        }


        public async Task<CustomerEntity> GetCustomerById(Guid id)
        {
            using (IDbConnection connection = new SqlConnection(_dbConnectionStringConfig.SqlServerConnectionString))
            {
                connection.Open();
                try
                {
                    var customer = await connection.QuerySingleAsync<Customer>
                        (@"select * from Customer where Id =@id", new{ id = id});
                    
                    return customer.ToEntity();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                return null;
            }
        }

        public async Task<List<CustomerEntity>> GetCustomers()
        {
            using (IDbConnection connection = new SqlConnection(_dbConnectionStringConfig.SqlServerConnectionString))
            {
                connection.Open();
                try
                {
                    var customers = await connection.QueryAsync<Customer>
                        (@"select * from Customer");

                    var list = new List<CustomerEntity>();
                    foreach (var customer in customers)
                    {
                        list.Add(customer.ToEntity());
                    }

                    return list;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                return null;
            }
        }

        public async Task<CustomerEntity> UpdateCustomer(CustomerEntity customerEntity)
        {
            
            using (IDbConnection connection = new SqlConnection(_dbConnectionStringConfig.SqlServerConnectionString))
            {
                connection.Open();
                try
                {
                    Customer customer = new Customer();

                    if (customerEntity.IsTransient())
                    {
                        customerEntity = new CustomerEntity(Guid.NewGuid(), customerEntity.Name);
                        customer.FromEntity(customerEntity);
                        await connection.ExecuteAsync(@"insert Customer(Id, Name) values (@Id, @Name)", customer);
                    }
                    else
                    {
                        customer.FromEntity(customerEntity);
                        await connection.ExecuteAsync(@"update Customer Name = @Name where Id = @Id", customer);
                    }

                    return await GetCustomerById(customerEntity.Id);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                return null;
            }
        }

        public async Task DeleteCustomer(Guid id)
        {
            using (IDbConnection connection = new SqlConnection(_dbConnectionStringConfig.SqlServerConnectionString))
            {
                connection.Open();
                try
                {
                    await connection.ExecuteAsync(@"delete from Customer where Id = @id", new { id = id.ToString()});
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
        }
    }
}
