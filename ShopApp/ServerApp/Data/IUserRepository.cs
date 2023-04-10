using System.Threading.Tasks;
using ServerApp.DTO;
using ServerApp.Models;

namespace ServerApp.Data
{
    public interface IUserRepository
    {
         Task<UserForUserPageDTO> GetUserById(int id);
         Task<User> GetUserByIdAsync(int userId);
          Task<bool> SaveChangesAsync();
    }
}