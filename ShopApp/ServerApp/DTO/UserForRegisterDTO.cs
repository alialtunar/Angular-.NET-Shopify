using System.ComponentModel.DataAnnotations;

namespace ServerApp.DTO
{
    public class UserForRegisterDTO
    {
        public string Name { get; set; }
         public string UserName { get; set; }
         public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public bool IsSupplier { get; set; }

         public string CompanyName { get; set; }
    
    }
#nullable enable
}