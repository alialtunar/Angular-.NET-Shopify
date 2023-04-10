using System;
using System.Collections.Generic;
using ServerApp.Models;

namespace ServerApp.DTO
{
    public class ProductForListDTO
    {     
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int StockCount { get; set; }
    public DateTime CreatedDate { get; set; }
   
     public ImagesForDetailsDTO Image { get; set; }

     
    

    }
}