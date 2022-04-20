using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Contracts;
using Northwind.Entities.Models;
using Northwind.Contracts;
using Northwind.Entities.DataTransferObject;
using System.Collections.Generic;
using Northwind.Entities.Models;

namespace Northwind.Repository
{
    public class ServiceManager : IServiceManager
    {
        private readonly IRepositoryManager _repository;

        public ServiceManager(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public Tuple<int, OrderDetail, string> AddToCart(int productId, int? qty, string custId)
        {
            Product product = new Product();
            Order order = new Order();
            OrderDetail orderDetail = new OrderDetail();

            product = _repository.Product.GetProduct(productId, trackChanges: false);
            order = _repository.Order.GetAllOrder(trackChanges: true).Where(x => x.CustomerId == custId && x.ShippedDate == null).SingleOrDefault();
            try
            {
                if (order == null)
                {
                    order = new Order();
                    order.CustomerId = custId;
                    order.OrderDate = DateTime.Now;

                    _repository.Order.CreateOrder(order);
                    _repository.Save();
                }
                
                orderDetail = _repository.OrderDetails.GetOrderDetails(order.OrderId, productId, trackChanges: true);
                if (orderDetail == null)
                {
                    orderDetail = new OrderDetail();
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.ProductId = productId;
                    orderDetail.UnitPrice = (decimal)((decimal)product.UnitPrice * qty);
                    orderDetail.Quantity = (short) qty;
                    _repository.OrderDetails.CreateOrderDetails(orderDetail);
                    _repository.Save();
                }
                else
                {
                        orderDetail.Quantity += (short)qty;
                        orderDetail.UnitPrice += (decimal)((decimal)product.UnitPrice * qty);
                        _repository.OrderDetails.UpdateOrderDetails(orderDetail);
                        _repository.Save();
                }
                return Tuple.Create(1, orderDetail, "success");
            }
            catch (Exception ex)
            {
                return Tuple.Create(-1, orderDetail, ex.Message);
            }
        }

        public Tuple<int, IEnumerable<Product>, string > GetAllProduct(bool trackChanges)
        {
            IEnumerable<Product> products1 = null;
            try
            {
                IEnumerable<Product> products = _repository.Product.GetAllProduct(trackChanges: false);
                return Tuple.Create(1, products, "success");
            }
            catch (Exception ex)
            {
                return Tuple.Create(-1, products1, ex.Message);
            }
        }

        
    }
}
