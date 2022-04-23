using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Entities.Models;
using Northwind.Entities.DataTransferObject;

namespace Northwind.Contracts
{
    public interface IServiceManager
    {
        Tuple<int, IEnumerable<Product>, string> GetAllProduct(bool trackChanges);
        Tuple<int, OrderDetail, string> AddToCart(int productId, int? qty, string custId );

        Order? CheckOut(int orderId);
       /* Tuple <int, Order, string> CheckOut(int orderId);*/
        Tuple<int, Order?, string> Shipped(ShippedDto shippedDto, int orderId);
    }
}
