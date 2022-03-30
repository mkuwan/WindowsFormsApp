using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp.Applications.CustomerService
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetCustomersAsync();
    }
}
