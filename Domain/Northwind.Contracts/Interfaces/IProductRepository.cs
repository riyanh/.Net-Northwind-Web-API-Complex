using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Entities.Models;

namespace Northwind.Contracts.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProduct(bool trackChanges);

        Product GetProduct(int ProductId, bool trackChanges);
        
    }
}
