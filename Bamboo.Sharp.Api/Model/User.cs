using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Bamboo.Sharp.Api.Model
{
    public class Users : List<User> { }

    public class User
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
    }
}
