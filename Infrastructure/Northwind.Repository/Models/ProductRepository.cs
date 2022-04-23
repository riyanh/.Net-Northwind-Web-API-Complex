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
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateProductUpdate(Product product)
        {
            Update(product);
        }

        public IEnumerable<Product> GetAllProduct(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.ProductId)
            .ToList();

        public Product GetProduct(int OrderId, bool trackChanges) =>
          FindByCondition(c => c.ProductId.Equals(OrderId), trackChanges).SingleOrDefault();


    }
}
