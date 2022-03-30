using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp.Domain.DapperRepository;

namespace WindowsFormsApp.Applications.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<CustomerDto>> GetCustomersAsync()
        {
            var list = await _customerRepository.GetCustomers();

            List<CustomerDto> customers = new List<CustomerDto>();
            
            list.ForEach(x =>
            {
                CustomerDto dto = new CustomerDto();
                customers.Add(dto.FromEntity(x));
            });

            return customers;
        }
    }
}
