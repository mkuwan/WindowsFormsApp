using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp.Domain.Models;

namespace WindowsFormsApp.Applications.CustomerService
{
    public class CustomerDto
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }

        public CustomerDto FromEntity(CustomerEntity customerEntity)
        {
            this.CustomerId = customerEntity.Id;
            this.CustomerName = customerEntity.Name;

            return this;
        }

        public CustomerEntity ToEntity()
        {
            return new CustomerEntity(this.CustomerId, this.CustomerName);
        }
    }
}
