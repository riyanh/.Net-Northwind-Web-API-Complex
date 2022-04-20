using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Contracts.Interfaces;
using Northwind.Entities.Contexts;
using Northwind.Entities.Models;
using Northwind.Entities.DataTransferObject;

namespace Northwind.Repository.Models
{
    public class OrderDetailsRepository : RepositoryBase<OrderDetail>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateOrderDetails(OrderDetail orderDetail)
        {
            Create(orderDetail);
        }

        public void DeleteOrderDetails(OrderDetail orderDetail)
        {
            Delete(orderDetail);
        }

        public IEnumerable<OrderDetail> GetAllOrderDetails(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.OrderId)
            .ToList();

        /*public IEnumerable<OrdersDetailDto> GetAllOrderDetails(bool trackChanges)
        {
            var result = (from od in repositoryContext.OrderDetails
                          join o in repositoryContext.Orders on od.OrderId equals o.OrderId
                          join p in repositoryContext.Products on od.ProductId equals p.ProductId
                          select new OrderDetail
                          {
                              ProductId = od.ProductId,
                              Quantity = od.Quantity,
                              UnitPrice = od.UnitPrice,
                              Discount  = od.Discount,
                              EmployeeId = o.EmployeeId,
                              CustomerId = o.CustomerId

                          });
            return (IEnumerable<OrdersDetailDto>)result;
        }*/

        public OrderDetail GetOrderDetails(int OrderId, int productId, bool trackChanges) =>
          FindByCondition(c => c.OrderId.Equals(OrderId) && c.ProductId.Equals(productId), trackChanges).SingleOrDefault();

        public void UpdateOrderDetails(OrderDetail orderDetail)
        {
           Update(orderDetail);
        }

        /*IEnumerable<OrderDetail> IOrderDetailsRepository.GetAllOrderDetails(bool trackChanges)
        {
            throw new NotImplementedException();
        }*/
    }
}
