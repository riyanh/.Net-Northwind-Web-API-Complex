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
                        orderDetail.Quantity = (short)(orderDetail.Quantity + qty);
                        orderDetail.UnitPrice = (decimal)(orderDetail.UnitPrice + (orderDetail.UnitPrice * qty));
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

      /*  public Tuple<int, Order, string> CheckOut(int orderId)
        {
            Order order1 = null;
            try
            {
                var order = _repository.Order.GetOrder(orderId, trackChanges: true);
                if (order == null)
                {
                    return Tuple.Create(-1, order, "Order Not Found");
                }
                order = new Order();
                order.RequiredDate = DateTime.Now;
                List<OrderDetail> orderDetail = _repository.OrderDetails.GetAllOrderDetails(trackChanges: true).Where(o => o.OrderId == orderId).ToList();
                foreach (var item in orderDetail)
                {
                    var product = _repository.Product.GetProduct(item.ProductId, trackChanges: true);
                    product.UnitsInStock -= item.Quantity;
                    _repository.Product.CreateProductUpdate(product);
                    _repository.Save();
                    
                }
                _repository.Order.UpdateOrder(order);
                _repository.Save();
                return Tuple.Create(1, order, "Succes");
            }
            catch (Exception ex)
            {
                return Tuple.Create(-1, order1, "Success");
            }
        }*/

        public Order CheckOut(int orderId)
        {
            //Order order = new Order();

            try
            {
                var order = _repository.Order.GetOrder(orderId, trackChanges: true);
                if (order == null)
                {
                    return order;
                }
                order = new Order();
                order.RequiredDate = DateTime.Now;
                List<OrderDetail> orderDetails = _repository.OrderDetails.GetAllOrderDetails(trackChanges: true).Where(o => o.OrderId == orderId).ToList();
                foreach (var item in orderDetails)
                {
                    var product = _repository.Product.GetProduct(item.ProductId, trackChanges: true);
                    product.UnitsInStock = (short?)(product.UnitsInStock - item.Quantity);
                    _repository.Product.CreateProductUpdate(product);
                    _repository.Save();
                }
                _repository.Order.UpdateOrder(order);
                _repository.Save();
                return order;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error", ex);
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

        public Tuple<int, Order, string> Shipped(ShippedDto shippedDto, int orderId)
        {
            var order = _repository.Order.GetOrder(orderId, trackChanges: true);
            //var customer = _repository.Customers.GetCustomer(order.CustomerId, trackChanges: true);

            if (order == null)
            {
                Tuple.Create(-1, order, "Not Found" );
            }
            else
            {
               /* order = new Order();
                order.ShipAddress = customer.Address;
                order.ShipCity = customer.City;
                order.ShipRegion = customer.Region;
                order.ShipPostalCode = customer.PostalCode;
                order.ShipCountry = customer.Country;
                order.ShipVia = shippedDto.ShipVia;
                order.Freight = shippedDto.Freight;
                order.ShipName = shippedDto.ShipName;
                order.ShippedDate = shippedDto.ShipDate;
                _repository.Order.UpdateOrder(order);
                _repository.Save();*/
            }
            return Tuple.Create(1, order, "success");
        }
    }
}
