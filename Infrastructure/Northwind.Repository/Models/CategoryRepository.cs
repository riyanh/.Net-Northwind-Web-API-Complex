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
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateCategory(Category category)
        {
            CreateCategory(category);
        }

        public void DeleteCategory(Category category)
        {
            DeleteCategory(category);
        }

        public IEnumerable<Category> GetAllCategory(bool trackChanges) =>
            FindAll(trackChanges)
                .OrderBy(c => c.CategoryName)
                .ToList();

        public Category GetCategory(int id, bool trackChanges) =>
            FindByCondition(c => c.CategoryId.Equals(id), trackChanges).SingleOrDefault();
        

        public void UpdateCategory(Category category)
        {
            UpdateCategory(category);
        }
    }
}
