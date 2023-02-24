using System;

namespace AspNetCoreAPI.Models.User
{
    public class User
    {
        public string Phone { get; set; } = string.Empty;          
        public string Token { get; set; } = string.Empty;         
        public DateTime Expires { get; set; }
    }
}
