using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp.Domain.Events
{
    public class CustomerDomainEventHandler : IDomainEventHandler<CustomerDomainEvent>
    {
        public delegate void DomainEventHandler(object sender, CustomerDomainEvent eventArgs);
        public event DomainEventHandler OnHandled;

        public void OnHandle(CustomerDomainEvent eventArgs)
        {
            OnHandled?.Invoke(this, eventArgs);
        }
    }
}
