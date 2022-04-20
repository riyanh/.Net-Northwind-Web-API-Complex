using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Entities.Models;

namespace Northwind.Contracts.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrder(bool trackChanges);

        Order GetOrder(int OrderId, bool trackChanges);
        void CreateOrder(Order order);
        void DeleteOrder(Order order);

        void UpdateOrder(Order order);
    }
}
