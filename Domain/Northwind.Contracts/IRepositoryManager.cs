using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Contracts.Interfaces;

namespace Northwind.Contracts
{
    public interface IRepositoryManager
    {
        ICategoryRepository Category { get; }

        ICustomersRepository Customers { get; }

        IOrderDetailsRepository OrderDetails { get; }
        IOrderRepository Order { get; }
        IProductRepository Product { get; }

        void Save();
        Task SaveAsync();
    }
}
