using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp.Domain.Models;

namespace WindowsFormsApp.Infrastructure.Models
{
    public class Customer
    {
        /// <summary>
        /// DBではStringのため、Dapperを使用した際にGuidが正しくParseできない
        /// そのため、このコンストラクタを使います
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Customer(string id, string name)
        {
            Name = name;
            Id = id;
        }

        public Customer(){}

        public string Id { get; set; }
        public string Name { get; set; }

        public CustomerEntity ToEntity()
        {
            return new CustomerEntity(new Guid(Id), Name);
        }

        public Customer FromEntity(CustomerEntity entity)
        {
            this.Id = entity.Id.ToString();
            this.Name = entity.Name;

            return this;
        }
    }
}
