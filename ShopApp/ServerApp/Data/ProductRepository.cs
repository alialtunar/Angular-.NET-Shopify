using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServerApp.DTO;
using ServerApp.Models;

namespace ServerApp.Data
{
    public class ProductRepository : IProductRepository
    {    

        private readonly ShopContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ShopContext shopContext, IMapper mapper)
        {
            _context=shopContext;
            _mapper = mapper;
        }
public async Task<Product> GetProductById(int id)
{
   var product = await _context.Products
    .Include(p => p.Images)
    .Include(p => p.ProductCategories)
        .ThenInclude(pc => pc.Category)
    .FirstOrDefaultAsync(i => i.ProductId == id);



    // productCategories alanındaki verileri çıkartarak sadece id'leri al ve adlarını güncelle
    product.ProductCategories = product.ProductCategories.Select(pc => new ProductCategory
    {
        CategoryId = pc.CategoryId,
        Category = new Category { CategoryId = pc.CategoryId, CategoryName = pc.Category.CategoryName }
    }).ToList();

    return product;
}

public async Task<IEnumerable<Product>> GetLastThreeProducts()
{
    return await _context.Products
        .Include(p => p.Images)
        .OrderByDescending(p => p.ProductId)
        .Take(3)
        .ToListAsync();
}



public async Task<IEnumerable<Product>> GetProducts(int categoryId, string name) {
    IQueryable<Product> query = _context.Products.Include(p => p.Images);

    if (categoryId > 0) {
        query = query.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId));
    }
    
    if (!string.IsNullOrEmpty(name))
    {
        string searchName = name.ToLower();
        query = query.Where(p => p.Name.ToLower().Contains(searchName));
    }

    return await query.ToListAsync();
}





        public async Task<IEnumerable<Product>> GetProductsByCategoryName(string categoryName)
        {
           
            var products = await _context.Products
                .Include(i=>i.Images)
                .Where(p => p.ProductCategories.Any(pc => pc.Category.CategoryName == categoryName))
                .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            var products = await _context.Products
            .Include(i=>i.Images)
            .Where(p=>p.Name==name)
            .ToListAsync();

            return products;
        }

     public async Task<IEnumerable<CategoryWithProductsDTO>> GetProductsForHome()
{
    var categoriesWithProducts = await _context.Categories
        .Include(c => c.ProductCategories)
            .ThenInclude(pc => pc.Product)
                .ThenInclude(p => p.Images)
        .ToListAsync();

   var result = categoriesWithProducts.Select(c => new CategoryWithProductsDTO
    {
        CategoryName = c.CategoryName,
        Products = c.ProductCategories
            .Where(pc => pc.Product.StockCount > 0)
            .OrderByDescending(pc => pc.Product.CreatedDate)
            .Take(8)
            .Select(pc => new ProductForListDTO
            {
                ProductId = pc.Product.ProductId,
                Name = pc.Product.Name,
                Description = pc.Product.Description,
                Price = pc.Product.Price,
                StockCount = pc.Product.StockCount,
                CreatedDate = pc.Product.CreatedDate,
                Image = _mapper.Map<ImagesForDetailsDTO>(pc.Product.Images.FirstOrDefault())
            })
    });

    return result;
}


     
    }
}