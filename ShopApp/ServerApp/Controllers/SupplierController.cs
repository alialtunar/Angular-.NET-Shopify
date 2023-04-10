using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Data;
using ServerApp.DTO;
using ServerApp.Models;
//using ServerApp.Models;

namespace ServerApp.Controllers
{
   [Authorize(Roles = "Supplier")]
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {

        private readonly ISupplierRepository _repository;
        public SupplierController(ISupplierRepository repository)
        {
            _repository = repository;
        }



    [HttpGet("{id}")]
        public IActionResult GetProductsBySupplierId(int id)
        {
            var products = _repository.GetProductsBySupplierId(id);
            return Ok(products);
        }

[HttpPost]
[Consumes("multipart/form-data")]
public async Task<IActionResult> CreateProduct([FromForm] Product model, [FromForm] IFormFile productImage, [FromForm] int categoryId)
{
    if (productImage != null && productImage.Length > 0)
    {
        var extension = Path.GetExtension(productImage.FileName);
        var randomName = string.Format($"{Guid.NewGuid()}{extension}");
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", randomName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await productImage.CopyToAsync(stream);
        }

        var image = new Image()
        {
            Name = randomName,
            Description = "Product image",
            DateAdded = DateTime.UtcNow,
            IsProfile = false,
            Product = null // You can set this later when you have the product ID
        };

        model.Images = new List<Image>() { image };
        
    }

    model.CreatedDate = DateTime.UtcNow;

    var productCategory = new ProductCategory
{
    CategoryId = categoryId
};

// Add the new ProductCategory object to the ProductCategories collection
model.ProductCategories.Add(productCategory);


    var result = await _repository.AddProduct(model);

    if (result)
    {
        return NoContent();
    }
    else
    {
        return BadRequest("Unable to create product");
    }
}




 [HttpPut("{id}")]
[Consumes("multipart/form-data")]
public async Task<IActionResult> UpdateProduct( [FromForm] Product model, [FromForm] IFormFile productImage, [FromForm] int categoryId)
{   
   

    if (productImage != null && productImage.Length > 0)
    {
        var extension = Path.GetExtension(productImage.FileName);
        var randomName = string.Format($"{Guid.NewGuid()}{extension}");
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", randomName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await productImage.CopyToAsync(stream);
        }

        var image = new Image()
        {
            Name = randomName,
            Description = "Product image",
            DateAdded = DateTime.UtcNow,
            IsProfile = false,
            Product = null // You can set this later when you have the product ID
        };

        model.Images = new List<Image>() { image };        
    }

     var productCategory = new ProductCategory
    {
        CategoryId = categoryId
    };


    model.CreatedDate = DateTime.UtcNow;



      model.ProductCategories.Add(productCategory);

    var result = await _repository.UpdateProduct(model.ProductId, model);

    if (result)
    {
        return NoContent();
    }
    else
    {
        return BadRequest(result);
    }
}


 [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repository.DeleteProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
       
    }
}