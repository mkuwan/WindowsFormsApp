using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp.Domain.Models;
using WindowsFormsApp.Domain.SeedWork;

namespace WindowsFormsApp.Domain.DapperRepository
{
    public interface ICustomerRepository : IRepository<CustomerEntity>
    {
        Task<CustomerEntity> GetCustomerById(Guid id);

        Task<List<CustomerEntity>> GetCustomers();

        Task<CustomerEntity> UpdateCustomer(CustomerEntity customerEntity);

        Task DeleteCustomer(Guid id);
    }
}
