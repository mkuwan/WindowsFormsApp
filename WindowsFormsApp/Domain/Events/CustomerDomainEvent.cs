using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp.Domain.Models;

namespace WindowsFormsApp.Domain.Events
{
    public class CustomerDomainEvent : IDomainEvent
    {
        public CustomerEntity CustomerEntity { get; }

        public CustomerDomainEvent(CustomerEntity customerEntity)
        {
            CustomerEntity = customerEntity;
        }
    }
}
