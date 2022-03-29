using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp.Domain.Events
{
    
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        // 以下はC#8.0以上でないと使えない・・・(T.T);
        //delegate void DomainEventHandler(object sender, T eventArgs);
        //event DomainEventHandler OnHandled;

        void OnHandle(T eventArgs);
    }
}
