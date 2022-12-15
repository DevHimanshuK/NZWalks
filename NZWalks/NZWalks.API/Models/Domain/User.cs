using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class User
    {
        
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Navigation Properties
        public List<User_Role> User_Roles { get; set; }
    }
}
