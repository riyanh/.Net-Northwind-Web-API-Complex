using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Contracts.Interfaces;
using Northwind.Entities.Contexts;
using Northwind.Entities.Models;

namespace Northwind.Repository.Models
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateOrder(Order order)
        {
            Create(order);
        }

        public void DeleteOrder(Order order)
        {
            Delete(order);
        }

        public IEnumerable<Order> GetAllOrder(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.OrderId)
            .ToList();

        public Order GetOrder(int OrderId, bool trackChanges) =>
          FindByCondition(c => c.OrderId.Equals(OrderId), trackChanges).SingleOrDefault();

        public void UpdateOrder(Order order)
        {
            Update(order);
        }
    }
}
