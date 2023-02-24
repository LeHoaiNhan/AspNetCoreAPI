using System.Collections.Generic;

namespace AspNetCoreAPI.Models.User
{
    public class UserConstants
    {

        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { Phone = "a", EmailAddress = "jason.admin@email.com", Password = "a", GivenName = "Jason", Surname = "Bryant", Role = "Administrator" },
            new UserModel() { Phone = "b", EmailAddress = "elyse.seller@email.com", Password = "b", GivenName = "Elyse", Surname = "Lambert", Role = "Seller" },
        };
    }
}
