using System.Collections.Generic;

namespace ServerApp.Models
{
    public class Customer:User
    {
     public List<Address> Addresses { get; set; }
    }
}