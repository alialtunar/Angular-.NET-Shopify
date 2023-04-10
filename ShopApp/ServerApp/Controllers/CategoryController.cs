using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Data;
using ServerApp.Models;
//using ServerApp.Models;

namespace ServerApp.Controllers
{   
   
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
         private readonly ICategoryRepository _repository;
        public CategoryController(ICategoryRepository repository)
        {
            _repository=repository;
        }



  [HttpGet]
public async Task<IActionResult> GetCategories()
{
    IEnumerable<Category> categories = await _repository.GetCategories();

   
    return Ok(categories);
}


       
    }
}