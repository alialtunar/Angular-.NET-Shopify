using System.Collections.Generic;
using System.Threading.Tasks;
using ServerApp.DTO;
using ServerApp.Models;

namespace ServerApp.Data
{
    public interface IProductRepository
    {
         Task<IEnumerable<Product>> GetProducts(int categoryId,string name);

         Task<Product> GetProductById(int id);
         Task<IEnumerable<Product>> GetLastThreeProducts();

         Task<IEnumerable<CategoryWithProductsDTO>> GetProductsForHome();

         Task<IEnumerable<Product>> GetProductsByCategoryName(string categoryName);

         Task<IEnumerable<Product>> GetProductsByName(string name);
    }
}