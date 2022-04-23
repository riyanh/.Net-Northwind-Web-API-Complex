using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Entities.Models;
using Northwind.Entities.RequestFeatures;

namespace Northwind.Contracts.Interfaces
{
    public interface ICategoryRepository
    {
        Task <IEnumerable<Category>> GetAllCategoryAsync(bool trackChanges);

        Task <Category> GetCategoryAsync(int id, bool trackChanges);

        void CreateCategoryAsync(Category category);
        void DeleteCategoryAsync(Category category);
        void UpdateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetPaginationCategoryAsync(CategoryParameters categoryParameters, bool trackChanges);

        Task<IEnumerable<Category>> GetSearchCategoryAsync(CategoryParameters categoryParameters, bool trackChanges);
    }
}
