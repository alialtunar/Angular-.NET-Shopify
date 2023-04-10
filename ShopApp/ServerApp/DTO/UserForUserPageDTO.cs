using System;
using System.ComponentModel.DataAnnotations;

namespace ServerApp.DTO
{
    public class UserForUserPageDTO
    {

         public int Id { get; set; }
       public string Name { get; set; }
        
        public string Gender { get; set; }

        public string UserName { get; set; }
         
         public string Email { get; set; }
        public string  PhoneNumber { get; set; }

        public string City { get; set; }     
        public string Country { get; set; }

        public string ImageUrl { get; set; }

       
    
    }
#nullable enable
}