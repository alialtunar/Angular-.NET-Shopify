using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Data
{
    public class CategoryRepository : ICategoryRepository
    {        
          private readonly ShopContext _context;

           public CategoryRepository(ShopContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Category>> GetCategories()
{
    return await _context.Categories
        .ToListAsync();
}

    }

   
      

   

   
}