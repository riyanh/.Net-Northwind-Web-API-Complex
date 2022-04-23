using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Contracts;
using Northwind.Contracts.Interfaces;
using Northwind.Entities.Contexts;
using Northwind.Repository.Models; 

namespace Northwind.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private ICategoryRepository _categoryRepository;
        private ICustomersRepository _customersRepository;
        private IOrderRepository _orderRepository;
        private IProductRepository _productRepository;
        private IOrderDetailsRepository _orderDetailsRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public ICategoryRepository Category
        {
            get {
                 if(_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_repositoryContext);
                }
                 return _categoryRepository;
                }
        }

        public ICustomersRepository Customers
        {
            get
            {
                if(_customersRepository == null)
                {
                    _customersRepository = new CustomersRepository(_repositoryContext);
                }
                return _customersRepository;
            }
        }

        public IOrderDetailsRepository OrderDetails
        {
            get
            {
                if(_orderDetailsRepository == null)
                {
                    _orderDetailsRepository = new OrderDetailsRepository(_repositoryContext);
                }
                return _orderDetailsRepository;
            }
        }

        public IOrderRepository Order
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_repositoryContext);
                }
                return _orderRepository;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_repositoryContext);
                }
                return _productRepository;
            }
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }

        public async Task SaveAsync() =>
            await _repositoryContext.SaveChangesAsync();
        
    }
}
