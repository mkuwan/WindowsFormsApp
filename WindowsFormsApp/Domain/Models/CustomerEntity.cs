using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp.Domain.Events;
using WindowsFormsApp.Domain.SeedWork;

namespace WindowsFormsApp.Domain.Models
{
    public sealed class CustomerEntity : EntityGuidId, IAggregateRoot
    {
        
        public CustomerEntity(Guid id, string name)
        {
            Name = name;
            Id = id;
        }


        public string Name { get; private set; }

        public void ChangeName(string name)
        {
            Name = name;
            AddDomainEvent(new CustomerDomainEvent(this));
        }
    }
}
