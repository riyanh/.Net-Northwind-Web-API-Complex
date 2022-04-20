using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Entities.Models;

namespace Northwind.Contracts.Interfaces
{
    public interface IOrderDetailsRepository
    {
        IEnumerable<OrderDetail> GetAllOrderDetails(bool trackChanges);

        OrderDetail GetOrderDetails(int OrderId, int productId,bool trackChanges);
        void CreateOrderDetails(OrderDetail order);
        void DeleteOrderDetails(OrderDetail order);

        void UpdateOrderDetails(OrderDetail order);
    }
}
