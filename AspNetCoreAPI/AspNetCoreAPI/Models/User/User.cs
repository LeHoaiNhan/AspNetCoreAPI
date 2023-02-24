using System;

namespace AspNetCoreAPI.Models.User
{
    public class User
    {
        public string Phone { get; set; } = string.Empty;          
        public string Token { get; set; } = string.Empty;

        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; } 

        public DateTime Expires { get; set; }
    }
}
