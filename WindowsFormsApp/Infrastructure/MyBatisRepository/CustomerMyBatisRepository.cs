using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBatisNet.DataMapper;
using WindowsFormsApp.Domain.DapperRepository;
using WindowsFormsApp.Domain.Models;
using WindowsFormsApp.Domain.SeedWork;
using WindowsFormsApp.Infrastructure.DBConnection;
using WindowsFormsApp.Infrastructure.Models;

namespace WindowsFormsApp.Infrastructure.MyBatisRepository
{
    public class CustomerMyBatisRepository: ICustomerRepository
    {
        private readonly WinFormDbConnectionStringConfig _dbConnectionConfig;
        private readonly ISqlMapper _entityMapper;

        public CustomerMyBatisRepository(WinFormDbConnectionStringConfig dbConnectionConfig)
        {
            _dbConnectionConfig = dbConnectionConfig;
            _entityMapper = Mapper.Instance();
            _entityMapper.DataSource.ConnectionString = _dbConnectionConfig.SqlServerConnectionString;
        }

        public IUnitOfWork UnitOfWork { get; }

        public async Task<CustomerEntity> GetCustomerById(Guid id)
        {
            Customer customer = new Customer();

            await Task.Run(() =>
            {
                customer = (Customer)_entityMapper.QueryForObject("GetCustomerById", id.ToString());
            });
            if (customer == null)
                return null;
            else
                return customer.ToEntity();
        }

        public async Task<List<CustomerEntity>> GetCustomers()
        {
            List<CustomerEntity> customerEntities = new List<CustomerEntity>();
            IList<Customer> customers = new List<Customer>();
            await Task.Run(() =>
            {
                customers = _entityMapper.QueryForList<Customer>("GetCustomerList", null);
            });

            foreach (var customer in customers)
            {
                customerEntities.Add(customer.ToEntity());
            }

            return customerEntities;
        }

        public async Task<CustomerEntity> UpdateCustomer(CustomerEntity customerEntity)
        {
            var customer = new Customer();

            if (customerEntity.IsTransient())
            {
                //新規
                await Task.Run(() =>
                {
                    customerEntity = new CustomerEntity(Guid.NewGuid(), customerEntity.Name);
                    customer.FromEntity(customerEntity);
                    _entityMapper.Insert("InsertCustomer", customer);
                });
            }
            else
            {
                // 更新
                await Task.Run(() =>
                {
                    customer.FromEntity(customerEntity);
                    _entityMapper.Update("UpdateCustomer", customer);
                });
            }

            return customer.ToEntity();
        }

        public async Task DeleteCustomer(Guid id)
        {
            await Task.Run(() =>
            {
                _entityMapper.Delete("DeleteCustomer", id.ToString());
            });
        }

    }
}
