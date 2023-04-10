using System.Threading.Tasks;
using ServerApp.DTO;
using ServerApp.Models;

namespace ServerApp.Data
{
    public class UserRepository : IUserRepository
    {

       private readonly ShopContext _context;

   public UserRepository(ShopContext context)
    {
        _context = context;
    }

       public async Task<UserForUserPageDTO> GetUserById(int id)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
    {
        return null;
    }

    var userDto = new UserForUserPageDTO
    {
        Name = user.Name,
        Gender = user.Gender,
        City = user.City,
        Country = user.Country,
        ImageUrl = user.ImageUrl,
        Id = user.Id,
        Email=user.Email,
        PhoneNumber=user.PhoneNumber,
        UserName=user.UserName
    };

    return userDto;
}

public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
    

    }
}