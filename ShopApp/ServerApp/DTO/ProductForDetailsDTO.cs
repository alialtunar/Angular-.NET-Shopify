
using System;
using System.Collections.Generic;
using ServerApp.Models;

namespace ServerApp.DTO
{
    public class ProductForDetailsDTO
    {
        
     public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int StockCount { get; set; }
    public DateTime? CreatedDate { get; set; }
     public int SupplierId { get; set; }

    public ICollection<ImagesForDetailsDTO> Images { get; set; }
   public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}