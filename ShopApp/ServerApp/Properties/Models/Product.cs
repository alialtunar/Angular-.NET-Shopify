using System;
using System.Collections.Generic;

namespace ServerApp.Models
{
    public class Product
    {
          public Product()
    {
        Images = new List<Image>();
        ProductCategories = new List<ProductCategory>();
    }
        
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int StockCount { get; set; }
    public DateTime CreatedDate { get; set; }
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    public ICollection<Image> Images { get; set; }
   public ICollection<ProductCategory> ProductCategories { get; set; }


  
    }
}