using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Models;
namespace ServerApp.Data
{
    public interface ISupplierRepository
    {
         Task<IEnumerable<Product>> GetMyProducts();

       IEnumerable<object> GetProductsBySupplierId(int id);

         Task<bool> AddProduct(Product product);
       Task<bool> UpdateProduct(int id, Product model);

       Task<Product> DeleteProduct(int id);
        Task<bool> SaveChanges();
        Task<Category> GetCategoryById(int categoryId);
    }
}