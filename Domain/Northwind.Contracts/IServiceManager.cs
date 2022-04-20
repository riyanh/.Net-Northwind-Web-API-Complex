using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Entities.Models;

namespace Northwind.Contracts
{
    public interface IServiceManager
    {
        Tuple<int, IEnumerable<Product>, string> GetAllProduct(bool trackChanges);
        Tuple<int, OrderDetail, string> AddToCart(int productId, int? qty, string custId );
    }
}
