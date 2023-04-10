using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Data;
using ServerApp.DTO;
using ServerApp.Models;
using ServerApp.Properties;
//using ServerApp.Models;

namespace ServerApp.Controllers
{  
    
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository repository,IMapper mapper)
        {
              _repository=repository;
            _mapper = mapper;
        }

  [HttpGet]
public async Task<ActionResult> GetProducts([FromQuery] UserQueryParams queryParams) {
    var products = await _repository.GetProducts(queryParams.categoryId,queryParams.name);
    var listOfProducts = _mapper.Map<IEnumerable<ProductForListDTO>>(products);
    return Ok(listOfProducts);
}

[HttpGet("last3")]
public async Task<ActionResult> GetLastThreeProducts()
{
    var products = await _repository.GetLastThreeProducts();
    var listOfProducts = _mapper.Map<IEnumerable<ProductForListDTO>>(products);
    return Ok(listOfProducts);
}


     
  
     [HttpGet("{id}")]
    public async Task<ActionResult> GetProduct(int id){
        var product= await _repository.GetProductById(id);
        var productDetail= _mapper.Map<ProductForDetailsDTO>(product);
        return Ok(productDetail);
    }


     [HttpGet("GetProductsByHome")]
public async Task<IActionResult> GetProductsForHome()
{
    var products = await _repository.GetProductsForHome();

    if (products == null || !products.Any())
    {
        return NotFound();
    }

    return Ok(products);
}




       
    }
}