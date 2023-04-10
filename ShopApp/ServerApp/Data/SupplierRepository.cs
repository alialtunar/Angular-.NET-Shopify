using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.DTO;
using ServerApp.Models;

namespace ServerApp.Data
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ShopContext _shopcontext;

        public SupplierRepository(ShopContext shopcontext)
        {
            _shopcontext = shopcontext;
        }
      public async Task<bool> AddProduct(Product model)
{
    try
    {
        // Add the product to the database
        _shopcontext.Products.Add(model);

        // Save changes to generate the ProductId value
        await _shopcontext.SaveChangesAsync();


        return true;
    }
    catch (Exception ex)
    {
        // hata durumunda hatayı göster
        Console.WriteLine(ex.Message);
        return false;
    }
}




public async Task<Category> GetCategoryById(int categoryId)
{
    return await _shopcontext.Categories.FindAsync(categoryId);
}




public IEnumerable<object> GetProductsBySupplierId(int supplierId)
{
    return _shopcontext.Products
        .Where(p => p.SupplierId == supplierId)
        .Select(p => new {
            Name = p.Name,
            Price = p.Price,
            SupplierId=p.SupplierId,
            StockCount=p.StockCount,
            ProductId = p.ProductId
            

        })
        .ToList();
}



        public Task<bool> SaveChanges()
        { 
            throw new System.NotImplementedException();
        }

       



       public async Task<Product> DeleteProduct(int productId)
        {
            var product = await _shopcontext.Products.FindAsync(productId);
            if (product == null)
            {
                return null;
            }

            _shopcontext.Products.Remove(product);
            await _shopcontext.SaveChangesAsync();

            return product;
        }

        public Task<IEnumerable<Product>> GetMyProducts()
        {
            throw new NotImplementedException();
        }

        public Supplier GetSupplierById(int id)
        {
            throw new NotImplementedException();
        }
public async Task<bool> UpdateProduct(int id, Product product)
{
    var existingProduct = await _shopcontext.Products
        .Include(p => p.Images)
        .Include(p => p.ProductCategories)
        .FirstOrDefaultAsync(p => p.ProductId == id);

    if (existingProduct == null)
    {
        return false;
    }

    existingProduct.Name = product.Name;
    existingProduct.Description = product.Description;
    existingProduct.Price = product.Price;
    existingProduct.CreatedDate = product.CreatedDate;
    

    if (product.Images != null && product.Images.Count > 0)
    {
        existingProduct.Images.Clear();
        foreach (var image in product.Images)
        {
            existingProduct.Images.Add(image);
        }
    }

    if (product.ProductCategories != null && product.ProductCategories.Count > 0)
    {
        existingProduct.ProductCategories.Clear();
        foreach (var category in product.ProductCategories)
        {
            existingProduct.ProductCategories.Add(category);
        }
    }

    await _shopcontext.SaveChangesAsync();

    return true;
}




    }

    }
