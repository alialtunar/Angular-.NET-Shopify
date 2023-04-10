using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models
{
    public class Supplier:User
    {    
        public string CompanyName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}