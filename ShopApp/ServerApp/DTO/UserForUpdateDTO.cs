using System.ComponentModel.DataAnnotations;

namespace ServerApp.DTO
{
    public class UserForUpdateDTO
    {
        public string Name { get; set; }
         public string UserName { get; set; }
         public string Email { get; set; }
         public string PhoneNumber { get; set; }
    
         public string ImageUrl { get; set; }
    }
}