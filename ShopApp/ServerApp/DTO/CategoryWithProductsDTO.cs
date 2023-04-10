using System.Collections.Generic;
using ServerApp.DTO;

public class CategoryWithProductsDTO
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public IEnumerable<ProductForListDTO> Products { get; set; }
}
